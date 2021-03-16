using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoInject.Core
{
    /// <summary>
    /// The module register
    /// </summary>
    public class ModuleRegister : IModuleRegister
    {
        /// <summary>
        /// The interface to skip register.
        /// </summary>
        private const string IModule = nameof(IModule);

        /// <summary>
        /// The service collection.
        /// </summary>
        private readonly IServiceCollection _serviceCollection;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ModuleRegister"/> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public ModuleRegister(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        /// <summary>
        /// Registers the service and its interface into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The lifetime.</param>
        public void Register(Type itype, Type type, ServiceLifetime lifetime)
        {
            // Check service type is not null.
            if(Equals(itype, null) || itype.FullName.Contains(IModule))
            {
                return;
            }
           
            _serviceCollection.Add(new ServiceDescriptor(itype, type, lifetime));
        }
    }
}
