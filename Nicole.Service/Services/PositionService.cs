using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class PositionService: BaseService, IPositionService
    {
        public PositionService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Insert(Position position)
        {
            DbContext.Positions.Add(position);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var model = GetPosition(id);
            if (model != null)
            {
                model.IsDeleted = true;
                Update();
            }
        }

        public Position GetPosition(Guid id)
        {
            return DbContext.Positions.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Position> GetPositions()
        {
            return DbContext.Positions.Where(n => !n.IsDeleted);
        }
    }
}
