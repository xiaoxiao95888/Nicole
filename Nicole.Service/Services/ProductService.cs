using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetProduct(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public IQueryable<Product> GetProducts()
        {
            return DbContext.Products.Where(n => !n.IsDeleted);
        }

        public Product GetProduct(Guid id)
        {
            return DbContext.Products.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(Product product)
        {
            DbContext.Products.Add(product);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
