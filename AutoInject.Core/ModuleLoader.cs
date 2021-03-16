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
        /// 
        /// </summary>
        private const char SplitPattern = '.';

        /// <summary>
        /// Load all included DLL to container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="externals">The registered assembly names.</param>
        public static void LoadContainer(this IServiceCollection services, IEnumerable<string> externals)
        {
            try
            {
                var registeredTypes = GetInjectedAssemblies(externals);
                var executableLocation = Assembly.GetEntryAssembly().Location;
                var assemblies = Directory
                           .GetFiles(Path.GetDirectoryName(executableLocation), SearchPattern, SearchOption.AllDirectories)
                           .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                           .ToList();

                var targetAssemblies = assemblies.SelectMany(n => n.DefinedTypes)
                                                 .Where(n => n.IsClass && registeredTypes.Any(m => n.FullName.Contains(m)));

                var containerConfig = new ContainerConfiguration()
                    .WithAssemblies(assemblies);

                using (var container = containerConfig.CreateContainer())
                {
                    var module = container.GetExport<IModule>();
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
                    builder.Append(loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        /// <summary>
        /// Gets injected assemblies.
        /// </summary>
        /// <param name="externals">The registered assembly names.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetInjectedAssemblies(IEnumerable<string> externals)
        {
            StackTrace st = new StackTrace(1, true);
            var declaringType = st.GetFrame(1).GetMethod().DeclaringType;
            var name = declaringType.FullName.Split(SplitPattern).FirstOrDefault();
            return new List<string> (externals)
            {
                name
            };
        }
    }
}
