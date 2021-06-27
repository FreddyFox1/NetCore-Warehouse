using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Warehouse.Services.Integrator.IntegratorAbstraction;

namespace Warehouse.Services.Integrator
{
    public partial class IntegratorService : IIntegrator
    {
        public async string ApiRequest(string api, string method)
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
