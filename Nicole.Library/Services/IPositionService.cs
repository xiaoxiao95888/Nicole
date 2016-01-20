using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IPositionService : IDisposable
    {
        void Insert(Position position);
        void Update();
        void Delete(Guid id);
        Position GetPosition(Guid id);
        IQueryable<Position> GetPositions();
        LeftNavigation GetLeftNavigation(Guid id);
        IQueryable<LeftNavigation> GetLeftNavigations();

    }
}
