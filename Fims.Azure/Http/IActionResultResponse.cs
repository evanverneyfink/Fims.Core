using System.Net.Http;
using Fims.Server.Api;
using Microsoft.AspNetCore.Mvc;

namespace Fims.Azure.Http
{
    public interface IActionResultResponse : IResponse
    {
        /// <summary>
        /// Gets the response as an <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <returns></returns>
        IActionResult AsActionResult();
    }
}