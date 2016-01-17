using Nicole.Library.Models;
using System;
using System.Linq;

namespace Nicole.Library.Services
{
    public interface IProductService : IDisposable
    {
        void Insert(Product product);
        void Update();
        void Delete(Guid id);
        Product GetProduct(Guid id);
        IQueryable<Product> GetProducts();
    }
}
