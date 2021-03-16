using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity;

namespace AutoInject.Mvc
{
    public class ModuleLoader
    {
        #region Unity Container

        private readonly static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              //var a = GetInjectedAssemblies(new List<string>());
              var unityContainer = new UnityContainer();
              LoadContainer(unityContainer);
              return unityContainer;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;

        #endregion Unity Container

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void LoadContainer(IUnityContainer container)
        {
            var directoryCategory = new DirectoryCatalog(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll");
            var registeredTypes = GetInjectedAssemblies(new List<string>());
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(n => n.DefinedTypes.Where(m => m.IsClass && registeredTypes.Any(o => m.FullName.Contains(o))));
            //var externals = ConfigurationManager.AppSettings[""];
            try
            {
                using (var aggregateCatalog = new AggregateCatalog())
                {
                    aggregateCatalog.Catalogs.Add(directoryCategory);
                    using (var compositionContainer = new CompositionContainer(aggregateCatalog))
                    {
                        var exports = compositionContainer.GetExport<IModule>();
                        var module = exports.Value;

                        var register = new ModuleRegister(container);
                        foreach (var assembly in assemblies)
                        {
                            module.Initialize(register, assembly);
                        }
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
            var declaringType = st.GetFrame(6).GetMethod().DeclaringType;
            var name = declaringType.FullName.Split('.').FirstOrDefault();
            return new List<string>(externals)
            {
                name, 
            };

        }
    }
}