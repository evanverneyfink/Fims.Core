using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fims.Azure
{
    public interface IFimsAzureResourceApi
    {
        /// <summary>
        /// Handles a request to a FIMS resource API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IActionResult> HandleRequest(HttpRequest request);
    }
}