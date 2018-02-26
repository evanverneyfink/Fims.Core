using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public interface IWorker
    {
        Task Execute(object input);
    }

    public interface IWorker<in T> : IWorker
    {
        Task Execute(T input);
    }
}
