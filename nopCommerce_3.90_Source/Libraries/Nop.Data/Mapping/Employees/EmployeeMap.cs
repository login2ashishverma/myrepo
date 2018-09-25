using Nop.Core.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Employees
{
    public partial class EmployeeMap : NopEntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            this.ToTable("Employee");
            this.HasKey(v => v.Id);

            this.Property(v => v.Name).IsRequired().HasMaxLength(400);
            this.Property(v => v.EmailId).HasMaxLength(400);
          
        }
    }
}
