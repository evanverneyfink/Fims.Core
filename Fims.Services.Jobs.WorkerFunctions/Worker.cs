using System.Threading.Tasks;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public abstract class Worker<T> : IWorker<T>
    {
        Task IWorker.Execute(object input)
        {
            return Execute((T)input);
        }

        public abstract Task Execute(T input);
    }
}