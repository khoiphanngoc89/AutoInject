using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace AutoInject.Core
{
    /// <summary>
    /// The module loader
    /// </summary>
    public static class ModuleLoader
    {
        /// <summary>
        /// The search pattern.
        /// </summary>
        private const string SearchPattern = "*.dll";

        /// <summary>
        /// Load all included DLL to container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="registeredAssemblyNames">The registered assembly names.</param>
        public static void LoadContainer(this IServiceCollection services, IEnumerable<string> registeredAssemblyNames)
        {
            try
            {
                var registeredTypes = GetInjectedAssemblies(registeredAssemblyNames);
                var executableLocation = Assembly.GetEntryAssembly().Location;
                var assemblies = Directory
                           .GetFiles(Path.GetDirectoryName(executableLocation), SearchPattern, SearchOption.AllDirectories)
                           .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                           .ToList();

                var targetAssemblies = assemblies.SelectMany(n => n.DefinedTypes).Where(m =>
                {
                    foreach (var registerType in registeredTypes)
                    {
                        return m.IsClass && m.FullName.StartsWith(registerType);
                    }

                    return false;
                });

                var containerConfig = new ContainerConfiguration()
                    .WithAssemblies(assemblies);

                using (var container = containerConfig.CreateContainer())
                {
                    var module = container.GetExport<IModule>();
                    module.RegisterAssemblyNames(registeredAssemblyNames);
                    var register = new ModuleRegister(services);
                    foreach (var targetAssembly in targetAssemblies)
                    {
                        module.Initialize(register, targetAssembly);
                    }
                }
            }
            catch (ReflectionTypeLoadException typeLoadException)
            {
                var builder = new StringBuilder();
                foreach (Exception loaderException in typeLoadException.LoaderExceptions)
                {
                    builder.AppendFormat("{0}\n", loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        /// <summary>
        /// Gets injected assemblies.
        /// </summary>
        /// <param name="registeredAssemblyNames">The registered assembly names.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetInjectedAssemblies(IEnumerable<string> registeredAssemblyNames)
        {
            StackTrace st = new StackTrace(1, true);
            var declaringType = st.GetFrame(1).GetMethod().DeclaringType;
            var name = declaringType.FullName.Split('.').FirstOrDefault();
            var lst = new List<string>
            {
                name
            };
            lst.AddRange(registeredAssemblyNames);
            return lst;
        }
    }
}
