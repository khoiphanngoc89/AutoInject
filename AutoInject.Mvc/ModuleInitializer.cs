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
    [Export(typeof(IModule))]
    public class ModuleInitializer : IModule
    {
        public void Initialize(IModuleRegister moduleRegister, TypeInfo type)
        {
            if (Equals(type, null))
            {
                return;
            }

            moduleRegister.Register(type.GetInterfaces().FirstOrDefault(), type, false);
        }
    }
}
