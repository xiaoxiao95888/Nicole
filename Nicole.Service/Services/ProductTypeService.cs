using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class ProductTypeService : BaseService, IProductTypeService
    {
        public ProductTypeService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetProductType(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public IQueryable<ProductType> GeProductTypes()
        {
            return DbContext.ProductTypes.Where(n => !n.IsDeleted);
        }

        public ProductType GetProductType(Guid id)
        {
            return DbContext.ProductTypes.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(ProductType productType)
        {
            DbContext.ProductTypes.Add(productType);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
