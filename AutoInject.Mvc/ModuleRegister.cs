using System;
using Unity;

namespace AutoInject.Mvc
{
    public class ModuleRegister : IModuleRegister
    {
        /// <summary>
        /// The interface to skip register.
        /// </summary>
        private const string IModule = nameof(IModule);

        /// <summary>
        /// The container.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public ModuleRegister(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Registers the service and its interface into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="isSingleton">The is singleton.</param>
        public void Register(Type itype, Type type, bool isSingleton)
        {
            if (Equals(itype, null) || string.IsNullOrWhiteSpace(itype.FullName) || itype.FullName.Contains(IModule))
            {
                return;
            }

            switch(isSingleton)
            {
                case true:
                    _container.RegisterSingleton(itype, type);
                    break;
                default:
                    _container.RegisterType(itype, type);
                    break;
            }    
        }
    }
}