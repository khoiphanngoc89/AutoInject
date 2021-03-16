using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoInject.Core
{
    /// <summary>
    /// Defines the module register
    /// </summary>
    public interface IModuleRegister
    {
        /// <summary>
        /// Registers the service and its interface into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The lifetime.</param>
        void Register(Type defineType, Type implementationType, ServiceLifetime lifetime);
    }
}
