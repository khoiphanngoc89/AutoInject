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
        /// <summary>
        /// The search pattern.
        /// </summary>
        private const string SearchPattern = "*.dll";

        /// <summary>
        /// The inject DLL.
        /// </summary>
        private const string InjectDlls = nameof(InjectDlls);

        /// <summary>
        /// The split pattern.
        /// </summary>
        private const char SplitPattern = '.';

        /// <summary>
        /// The split inject DLL pattern.
        /// </summary>
        private const char SplitInjectDllsPattern = ';';

        #region Unity Container

        /// <summary>
        /// 
        /// </summary>
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
            var directoryCategory = new DirectoryCatalog(AppDomain.CurrentDomain.RelativeSearchPath, SearchPattern);
            var registeredTypes = GetAssemblies();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .SelectMany(n => n.DefinedTypes.Where(m => m.IsClass && registeredTypes.Any(o => m.FullName.Contains(o))));
            //
            try
            {
                using (var aggregateCatalog = new AggregateCatalog())
                {
                    aggregateCatalog.Catalogs.Add(directoryCategory);
                    using (var compositionContainer = new CompositionContainer(aggregateCatalog))
                    {
                        var export = compositionContainer.GetExport<IModule>();
                        if(Equals(export, null))
                        {
                            throw new ReflectionTypeLoadException(new Type[] { }, new Exception[] { }, "Cannot load IModule.");
                        }    

                        var module = export.Value;
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
        private static IEnumerable<string> GetAssemblies()
        {
            var externals = GetRegisteredExternal();
            var st = new StackTrace(1, true);
            var declaringType = st.GetFrame(6).GetMethod().DeclaringType;
            var name = declaringType.FullName.Split(SplitPattern).FirstOrDefault();
            return new List<string>(externals)
            {
                name, 
            };
        }

        /// <summary>
        /// Gets registered external DLL
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetRegisteredExternal()
        {
            var raw = ConfigurationManager.AppSettings[InjectDlls];
            return raw?.Split(SplitInjectDllsPattern).ToList() ?? new List<string>();

        }
    }
}