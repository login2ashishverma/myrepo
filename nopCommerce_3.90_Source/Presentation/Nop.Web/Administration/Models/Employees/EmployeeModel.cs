using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Models.Common;
using Nop.Admin.Validators.Employees;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Employees
{
    [Validator(typeof(EmployeeValidator))]
    public partial class EmployeeModel : BaseNopEntityModel
    {
        public EmployeeModel()
        {
            //if (PageSize < 1)
            //{
            //    PageSize = 5;
            //}
            Address = new AddressModel();

            //Locales = new List<EmployeeLocalizedModel>();
            //AssociatedCustomers = new List<AssociatedCustomerInfo>();
        }

        [NopResourceDisplayName("Admin.Employees.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Employees.Fields.Email")]
        [AllowHtml]
        public string EmailId { get; set; }

        [NopResourceDisplayName("Admin.Employees.Fields.Designation")]
        [AllowHtml]
        public string Designation { get; set; }

        public string PanNumber { get; set; }

        public string Gender { get; set; } 

        [NopResourceDisplayName("Admin.Employees.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [NopResourceDisplayName("Admin.Employees.Fields.AdminComment")]
        [AllowHtml]
        public string AdminComment { get; set; }

        public AddressModel Address { get; set; }

        [NopResourceDisplayName("Admin.Employees.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Employees.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        //[NopResourceDisplayName("Admin.Employees.Fields.AssociatedCustomerEmails")]
        //public IList<AssociatedCustomerInfo> AssociatedCustomers { get; set; }
        [UIHint("DateNullable")]
        public DateTime? DateOFBirth { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        [NopResourceDisplayName("Admin.Employees.Fields.BankName")]
        public string BankName { get; set; }
        [NopResourceDisplayName("Admin.Employees.Fields.BrachAddress")]
        public string BrachAddress { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Employees.Fields.AccountNumber")]
        public string AccountNumber { get; set; }
         [NopResourceDisplayName("Admin.Employees.Fields.SwitchCode")]
        public string SwitchCode { get; set; }
        [NopResourceDisplayName("Admin.Employees.Fields.IfscCode")]
        public string IfscCode { get; set; }



        #region Nested classes

        //public class AssociatedCustomerInfo : BaseNopEntityModel
        //{
        //    public string Email { get; set; }
        //}

        #endregion

    }

   
}