using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.DependencyInjection
{
    public class InjectionScopeManager : IInjectionScopeManager
    {
        /// <summary>
        /// Instantiates an <see cref="InjectionScopeManager"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        public InjectionScopeManager(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        /// <summary>
        /// Gets the underlying service provider
        /// </summary>
        private IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the collection of scopes mapped to function instance IDs
        /// </summary>
        private ConcurrentDictionary<Guid, IServiceScope> Scopes { get; } = new ConcurrentDictionary<Guid, IServiceScope>();

        /// <summary>
        /// Begins a scope when a function starts
        /// </summary>
        /// <param name="id"></param>
        public void BeginScope(Guid id) => Scopes.TryAdd(id, ServiceProvider.CreateScope());

        /// <summary>
        /// Ends a scope when a function ends
        /// </summary>
        /// <param name="id"></param>
        public void EndScope(Guid id)
        {
            Scopes.TryRemove(id, out var scope);
            scope.Dispose();
        }

        /// <summary>
        /// Gets the service provider for a function instance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider(Guid id) =>
            Scopes.TryGetValue(id, out var scope) ? scope.ServiceProvider : throw new Exception($"Scope with ID {id} not found.");
    }
}