using System;
using System.Collections.Generic;
using System.Linq;
using Final_Project_Cache;
namespace CacheServer
{
    public class ProductRepository : IRepository
    {
        private List<Product> products = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 1500,
                Description = "Gaming laptop"
            },
            new Product
            {
                Id = 2,
                Name = "Phone",
                Price = 800,
                Description = "Smartphone"
            },
            new Product
            {
                Id = 3,
                Name = "Headphones",
                Price = 100,
                Description = "Wireless headphones"
            }
        };

        public List<Product> GetAll()
        {
            return products;
        }

        public Product GetById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id)!;
        }

        public void Add(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            Product existingProduct = GetById(product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
            }
        }

        public void Delete(int id)
        {
            Product product = GetById(id);

            if (product != null)
            {
                products.Remove(product);
            }
        }
    }
}

