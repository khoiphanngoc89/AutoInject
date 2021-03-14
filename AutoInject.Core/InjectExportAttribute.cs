using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace AutoInject.Core
{
    /// <summary>
    /// The inject export attribute
    /// </summary>
    public class InjectExportAttribute : ExportAttribute
    {
        /// <summary>
        /// The service lifetime
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractType">The contact type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        public InjectExportAttribute(Type contractType, ServiceLifetime serviceLifetime) : base(contractType)
        {
            this.ServiceLifetime = serviceLifetime;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contact name.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        public InjectExportAttribute(string contractName, ServiceLifetime serviceLifetime) : base(contractName)
        {
            this.ServiceLifetime = serviceLifetime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contact name.</param>
        /// <param name="contractType">The contact type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        public InjectExportAttribute(string contractName, Type contractType, ServiceLifetime serviceLifetime) : base(contractName, contractType)
        {
            this.ServiceLifetime = serviceLifetime;
        }
    }
}
