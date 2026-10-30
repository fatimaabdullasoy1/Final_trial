
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Final_Project_Cache;
using Final_Project_Cache;

namespace CacheServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductRepository repository = new ProductRepository();
            CacheService cache = new CacheService();

            HttpListener listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:5000/");

            listener.Start();

            Console.WriteLine("CACHE SERVER STARTED");
            Console.WriteLine("Listening on http://localhost:5000/");
            Console.WriteLine("Waiting for requests...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string method = request.HttpMethod;
                string path = request.Url!.AbsolutePath;

                string[] parts = path.Trim('/').Split('/');

                string result = "";

                try
                {
                    if (parts.Length == 1 && parts[0] == "products")
                    {
                        if (method == "GET")
                        {
                            result = JsonSerializer.Serialize(repository.GetAll());
                        }
                        else if (method == "POST")
                        {
                            StreamReader reader = new StreamReader(request.InputStream);
                            string body = reader.ReadToEnd();

                            Product product = JsonSerializer.Deserialize<Product>(body)!;

                            product.Id = repository.GetAll().Count + 1;

                            repository.Add(product);

                            result = JsonSerializer.Serialize(product);
                        }
                    }
                    else if (parts.Length == 2 && parts[0] == "products")
                    {
                        int id = int.Parse(parts[1]);

                        string key = "product_" + id;

                        if (method == "GET")
                        {
                            Product product = cache.Get(key);

                            if (product != null)
                            {
                                Console.WriteLine("CACHE HIT: " + key);
                            }
                            else
                            {
                                Console.WriteLine("CACHE MISS: " + key);

                                product = repository.GetById(id);

                                if (product != null)
                                {
                                    cache.Set(key, product);
                                }
                            }

                            result = JsonSerializer.Serialize(product);
                        }
                        else if (method == "PUT")
                        {
                            StreamReader reader = new StreamReader(request.InputStream);
                            string body = reader.ReadToEnd();

                            Product product = JsonSerializer.Deserialize<Product>(body)!;

                            product.Id = id;

                            repository.Update(product);

                            cache.Remove(key);

                            result = JsonSerializer.Serialize(product);
                        }
                        else if (method == "DELETE")
                        {
                            repository.Delete(id);

                            cache.Remove(key);

                            result = "Product deleted successfully";
                        }
                    }
                    else
                    {
                        response.StatusCode = 404;
                        result = "Endpoint not found";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 400;
                    result = ex.Message;
                }

                byte[] buffer = Encoding.UTF8.GetBytes(result);

                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;
                response.ContentLength64 = buffer.Length;

                response.OutputStream.Write(buffer, 0, buffer.Length);

                response.Close();
            }
        }
    }
}
