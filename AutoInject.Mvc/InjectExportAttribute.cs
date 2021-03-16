using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInject.Mvc
{
    public class InjectExportAttribute : ExportAttribute
    {
        /// <summary>
        /// The service lifetime
        /// </summary>
        public bool IsSingleton { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractType">The contact type.</param>
        /// <param name="isSingleton">The service lifetime.</param>
        public InjectExportAttribute(Type contractType, bool isSingleton) : base(contractType)
        {
            this.IsSingleton = isSingleton;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contact name.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        public InjectExportAttribute(string contractName, bool isSingleton) : base(contractName)
        {
            this.IsSingleton = isSingleton;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contact name.</param>
        /// <param name="contractType">The contact type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        public InjectExportAttribute(string contractName, Type contractType, bool isSingleton) : base(contractName, contractType)
        {
            this.IsSingleton = isSingleton;
        }
    }
}
