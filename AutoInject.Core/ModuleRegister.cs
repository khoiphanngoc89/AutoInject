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
        /// <param name="serviceCollection"></param>
        public ModuleRegister(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        /// <summary>
        /// Adds service type into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The service lifetime.</param>
        public void Add(Type defineType, Type implementationType, ServiceLifetime lifetime)
        {
            // Check service type is not null.
            if(Equals(defineType, null) || defineType.FullName.Contains(IModule))
            {
                return;
            }

            var descriptor = new ServiceDescriptor(defineType, implementationType, lifetime);
            _serviceCollection.Add(descriptor);
        }
    }
}
