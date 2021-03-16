using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoInject.Mvc
{
    /// <summary>
    /// Define the module
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Initialize to get interface, implementation to register.
        /// </summary>
        /// <param name="moduleRegister">The module register.</param>
        /// <param name="type">The type.</param>
        void Initialize(IModuleRegister moduleRegister, TypeInfo type);
    }
}
