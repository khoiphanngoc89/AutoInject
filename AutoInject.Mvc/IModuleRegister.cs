using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInject.Mvc
{
    public interface IModuleRegister
    {
        void Register(Type itype, Type type, bool isSingleton);
    }
}
