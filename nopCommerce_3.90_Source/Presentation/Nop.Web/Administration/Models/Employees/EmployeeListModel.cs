using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Employees
{
    public partial class EmployeeListModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.Employees.List.SearchName")]
        [AllowHtml]
        public string SearchName { get; set; }
    }
}