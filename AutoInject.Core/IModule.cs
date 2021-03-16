using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoInject.Core
{
    /// <summary>
    /// Defines the module
    /// </summary>
    public interface IModule
    { 
        /// <summary>
        /// Initialize to get interface, implementation to register.
        /// </summary>
        /// <param name="moduleRegister">The module register.</param>
        /// <param name="type">The type.</param>
        /// <param name="lifetime">lifetime.</param>
        void Initialize(IModuleRegister moduleRegister, TypeInfo type);
    }
}
