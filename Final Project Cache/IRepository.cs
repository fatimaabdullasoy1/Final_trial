using System;
namespace Final_Project_Cache
{
    public interface IRepository
    {
        List<Product> GetAll();

        Product GetById(int id);

        void Add(Product product);

        void Update(Product product);

        void Delete(int id);
    }
}

