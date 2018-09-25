using FluentValidation;
using FluentValidation.Results;
using Nop.Admin.Models.Employees;
using Nop.Core.Domain.Employees;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Admin.Validators.Employees
{
    public partial class EmployeeValidator : BaseNopValidator<EmployeeModel>
    {
        public EmployeeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Employees.Fields.Name.Required"));

            RuleFor(x => x.EmailId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Employees.Fields.Email.Required"));
            RuleFor(x => x.EmailId).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            //RuleFor(x => x.PageSizeOptions).Must(ValidatorUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Employees.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            //Custom(x =>
            //{
            //    if (!x.AllowCustomersToSelectPageSize && x.PageSize <= 0)
            //        return new ValidationFailure("PageSize", localizationService.GetResource("Admin.Employees.Fields.PageSize.Positive"));

            //    return null;
            //});

            SetDatabaseValidationRules<Employee>(dbContext);
        }
    }
}