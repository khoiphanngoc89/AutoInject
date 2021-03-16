using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;

namespace AutoInject.Core
{
    /// <summary>
    /// The model initializer 
    /// </summary>
    [Export(typeof(IModule))]
    public class ModuleInitializer : IModule
    {
        /// <summary>
        /// Initialize to get interface, implementation to register.
        /// </summary>
        /// <param name="moduleRegister">The module register.</param>
        /// <param name="type">The type.</param>
        public void Initialize(IModuleRegister moduleRegister, TypeInfo type)
        {
            if(Equals(type, null))
            {
                return;
            }    

            // Check assembly name include in register assembly names
            moduleRegister.Register(type.GetInterfaces().FirstOrDefault(), type, this.GetLifetime(type));
        }

        private ServiceLifetime GetLifetime(TypeInfo type)
        {
            var raw = type.GetCustomAttribute(typeof(InjectExportAttribute));
            var attribute = (raw as InjectExportAttribute)?.ServiceLifetime;
            return attribute ?? ServiceLifetime.Scoped;
        }
    }
}
