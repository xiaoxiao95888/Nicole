using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IFaPiaoService : IDisposable
    {
        void Insert(FaPiao faPiao);
        void Update();
        void Delete(Guid id);
        FaPiao GetFaPiao(Guid id);
        IQueryable<FaPiao> GetFaPiaos();
    }
}
