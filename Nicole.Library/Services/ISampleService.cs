using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface ISampleService : IDisposable
    {
        void Insert(Sample sample);
        void Update();
        void Delete(Guid id);
        Sample GetSample(Guid id);
        IQueryable<Sample> GetSamples();
        IQueryable<string> GetSampleCodes();
    }
}
