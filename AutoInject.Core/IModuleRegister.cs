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
        /// Adds service type into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        void Add(Type defineType, Type implementationType, ServiceLifetime serviceLifetime);
    }
}
