using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AutoInject.Mvc
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
            if (Equals(type, null))
            {
                return;
            }

            moduleRegister.Register(type.GetInterfaces().FirstOrDefault(), type, IsSingleton(type));
        }

        /// <summary>
        /// Get is singleton from attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsSingleton(TypeInfo type)
        {
            var raw = type.GetCustomAttribute(typeof(InjectExportAttribute));
            var attribute = (raw as InjectExportAttribute)?.IsSingleton;
            return attribute ?? false;
        }
    }
}
