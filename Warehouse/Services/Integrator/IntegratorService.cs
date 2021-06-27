using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Warehouse.Services.Integrator.Abstraction;

namespace Warehouse.Services.Integrator
{
    public partial class IntegratorService : IIntegrator
    {
        public string ApiRequest(string api, string method)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string FilePath)
        {
            var Data = File.ReadAllText(FilePath);
            return Data;
        }

        public void WriteFile(string FilePath, string Data)
        {
            File.WriteAllText(FilePath, Data);
        }

    }
}
