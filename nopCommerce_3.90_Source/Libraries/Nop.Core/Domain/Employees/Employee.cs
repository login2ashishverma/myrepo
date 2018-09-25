using Nop.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Employees
{
    public partial class Employee : BaseEntity, ILocalizedEntity
    {
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string EmailId { get; set; }

        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the address identifier
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string PanNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        

        public DateTime DateOFBirth { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string BankName { get; set; }

        public string BrachAddress {get;set;}

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string AccountNumber { get; set; }

        public string SwitchCode {get;set;}

        public string IfscCode { get; set; }

    }

}
