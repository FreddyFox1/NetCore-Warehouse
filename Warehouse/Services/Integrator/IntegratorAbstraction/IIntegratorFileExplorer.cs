using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.Integrator.IntegratorAbstraction
{
    interface IIntegratorFileExplorer
    {
        void SaveFile(string FilePath);
        string ReadFile(string FilePath);
        void WriteFile (string FilePath, string Data);
        void DeleteFile(string FilePath);
    }
}
