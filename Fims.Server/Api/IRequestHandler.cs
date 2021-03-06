﻿using System.Threading.Tasks;

namespace Fims.Server.Api
{
    public interface IRequestHandler
    {
        /// <summary>
        /// Handles an HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResponse> HandleRequest(IRequest request);
    }
}