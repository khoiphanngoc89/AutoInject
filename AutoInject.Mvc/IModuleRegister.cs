using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInject.Mvc
{
    public interface IModuleRegister
    {
        /// <summary>
        /// Registers the service and its interface into container. 
        /// </summary>
        /// <param name="defineType">The define type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="isSingleton">The is singleton.</param>
        void Register(Type itype, Type type, bool isSingleton);
    }
}
