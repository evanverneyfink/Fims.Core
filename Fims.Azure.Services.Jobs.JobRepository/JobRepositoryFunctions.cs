using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fims.Azure.Services.Jobs.JobRepository
{
    public static class JobRepositoryFunctions
    {
        public static Task<IActionResult> HandleJobRepositoryRequest([Inject] IFimsAzureResourceApi resourceApi, HttpRequest request)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}
