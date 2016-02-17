using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class FaPiaoService : BaseService, IFaPiaoService
    {
        public FaPiaoService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetFaPiao(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public FaPiao GetFaPiao(Guid id)
        {
            return DbContext.FaPiaos.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<FaPiao> GetFaPiaos()
        {
            return DbContext.FaPiaos.Where(n => !n.IsDeleted);
        }

        public void Insert(FaPiao faPiao)
        {
            DbContext.FaPiaos.Add(faPiao);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
