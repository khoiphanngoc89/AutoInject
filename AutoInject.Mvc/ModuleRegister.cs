using System;
using Unity;

namespace AutoInject.Mvc
{
    public class ModuleRegister : IModuleRegister
    {
        private readonly IUnityContainer _container;

        public ModuleRegister(IUnityContainer container)
        {
            _container = container;
        }

        public void Register(Type itype, Type type, bool isSingleton)
        {
            if (Equals(itype, null) || itype.FullName.Contains("IModule"))
            {
                return;
            }
            _container.RegisterType(itype, type);
        }
    }
}