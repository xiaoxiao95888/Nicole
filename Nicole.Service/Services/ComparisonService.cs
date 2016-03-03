using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class ComparisonService : BaseService, IComparisonService
    {
        public ComparisonService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public IQueryable<LevelComparison> GetLevelComparisons()
        {
            return DbContext.LevelComparisons;
        }

        public IQueryable<PitchComparison> GetPitchComparisons()
        {
            return DbContext.PitchComparisons;
        }

        public IQueryable<ProductTypeComparison> GetProductTypeComparisons()
        {
            return DbContext.ProductTypeComparisons;
        }

        public IQueryable<VoltageComparison> GetVoltageComparisons()
        {
            return DbContext.VoltageComparisons;
        }
    }
}
