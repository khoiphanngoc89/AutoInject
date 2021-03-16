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
        /// The register assembly names.
        /// </summary>
        private IEnumerable<string> regAssemblyNames;

        /// <summary>
        /// Initialize to get interface, implementation to register.
        /// </summary>
        /// <param name="moduleRegister">The module register.</param>
        /// <param name="type">The type.</param>
        public void Initialize(IModuleRegister moduleRegister, TypeInfo type)
        {
            // Check assembly name include in register assembly names
            var (itype, serviceLifetime) = this.GetContactInfo(type);
            moduleRegister.Add(itype, type, serviceLifetime);
        }

        /// <summary>
        /// Initialize to get interface, implementation to register.
        /// </summary>
        /// <param name="moduleRegister">The module register.</param>
        /// <param name="type">The type.</param>
        /// <param name="lifetime">The life time.</param>
        public void Initialize(IModuleRegister moduleRegister, TypeInfo type, ServiceLifetime lifetime)
        {
            moduleRegister.Add(type, type, lifetime);
        }

        /// <summary>
        /// Registers name spaces
        /// </summary>
        /// <param name="assemblyNames">The assembly names.</param>
        public void RegisteredAssemblyNames(IEnumerable<string> assemblyNames)
        {
            this.regAssemblyNames = assemblyNames;
        }

        /// <summary>
        /// Get inject export attribute.
        /// </summary>
        /// <param name="type">The type of input assembly.</param>
        /// <returns>InjectExportAttribute</returns>
        private InjectExportAttribute GetInjectExportAttribute(TypeInfo type)
        {
            var customAttr = type.GetCustomAttribute(typeof(InjectExportAttribute));
            return customAttr as InjectExportAttribute;
        }

        /// <summary>
        /// Gets inject export information include contact type name and service lifetime.
        /// </summary>
        /// <param name="type">The type of input assembly.</param>
        /// <returns>(The contact type name, the service lifetime)</returns>
        private (string, ServiceLifetime) GetInjectExportInfo(TypeInfo type)
        {
            var injectExportAttr = this.GetInjectExportAttribute(type);
            return Equals(injectExportAttr, null) ?
                (string.Empty, ServiceLifetime.Scoped) :
                (injectExportAttr.ContractType.Name, injectExportAttr.ServiceLifetime);
        }

        /// <summary>
        /// Gets the contact type information include defined interface and service lifetime. 
        /// </summary>
        /// <param name="type">The type of input assembly.</param>
        /// <returns>(The type of defined interface, the service lifetime)</returns>
        private (Type, ServiceLifetime) GetContactInfo(TypeInfo type)
        {
            var (contactTypeName, serviceLifetime) = this.GetInjectExportInfo(type);
            var interfaces = type.GetInterfaces();

            return (string.IsNullOrWhiteSpace(contactTypeName) ?
                interfaces.FirstOrDefault() :
                interfaces.FirstOrDefault(n => string.Equals(n.Name, contactTypeName)), serviceLifetime);
        }
    }
}
