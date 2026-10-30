
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Final_Project_Cache;

namespace CacheClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();

            string url = "http://localhost:5000/products";

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== PRODUCT CLIENT =====");
                Console.WriteLine("1. Get All Products");
                Console.WriteLine("2. Get Product By ID");
                Console.WriteLine("3. Add Product");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("0. Exit");

                Console.Write("Choose: ");

                string choice = Console.ReadLine()!;

                try
                {
                    if (choice == "1")
                    {
                        string result = await client.GetStringAsync(url);

                        Console.WriteLine(result);
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Enter Product ID: ");
                        int id = int.Parse(Console.ReadLine()!);

                        string result = await client.GetStringAsync(url + "/" + id);

                        Console.WriteLine(result);
                    }
                    else if (choice == "3")
                    {
                        Product product = new Product();

                        Console.Write("Enter Name: ");
                        product.Name = Console.ReadLine()!;

                        Console.Write("Enter Price: ");
                        product.Price = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Enter Description: ");
                        product.Description = Console.ReadLine()!;

                        string json = JsonSerializer.Serialize(product);

                        StringContent content = new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json"
                        );

                        HttpResponseMessage response =
                            await client.PostAsync(url, content);

                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                    else if (choice == "4")
                    {
                        Console.Write("Enter Product ID: ");
                        int id = int.Parse(Console.ReadLine()!);

                        Product product = new Product();

                        product.Id = id;

                        Console.Write("Enter New Name: ");
                        product.Name = Console.ReadLine()!;

                        Console.Write("Enter New Price: ");
                        product.Price = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Enter New Description: ");
                        product.Description = Console.ReadLine()!;

                        string json = JsonSerializer.Serialize(product);

                        StringContent content = new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json"
                        );

                        HttpResponseMessage response =
                            await client.PutAsync(url + "/" + id, content);

                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                    else if (choice == "5")
                    {
                        Console.Write("Enter Product ID: ");
                        int id = int.Parse(Console.ReadLine()!);

                        HttpResponseMessage response =
                            await client.DeleteAsync(url + "/" + id);

                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                    else if (choice == "0")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            client.Dispose();
        }
    }
}

