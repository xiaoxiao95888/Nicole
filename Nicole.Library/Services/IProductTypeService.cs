using Nicole.Library.Models;
using System;
using System.Linq;

namespace Nicole.Library.Services
{
    public interface IProductTypeService : IDisposable
    {
        void Insert(ProductType productType);
        void Update();
        void Delete(Guid id);
        ProductType GetProductType(Guid id);
        IQueryable<ProductType> GeProductTypes();
    }
}
