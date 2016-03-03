using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IComparisonService : IDisposable
    {
        IQueryable<ProductTypeComparison> GetProductTypeComparisons();
        IQueryable<VoltageComparison> GetVoltageComparisons();
        IQueryable<LevelComparison> GetLevelComparisons();
        IQueryable<PitchComparison> GetPitchComparisons();
    }
}
