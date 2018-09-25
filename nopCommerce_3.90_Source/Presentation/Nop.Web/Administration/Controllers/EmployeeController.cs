using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Models.Employees;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Employees;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Employees;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
    public partial class EmployeeController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IEmployeeService _EmployeeService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPictureService _pictureService;
        private readonly IDateTimeHelper _dateTimeHelper;
        //private readonly EmployeeSettings _EmployeeSettings;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;

        #endregion

        #region Constructors

        public EmployeeController(ICustomerService customerService, 
            ILocalizationService localizationService,
            IEmployeeService EmployeeService, 
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            IPictureService pictureService,
            IDateTimeHelper dateTimeHelper,
            //EmployeeSettings EmployeeSettings,
            ICustomerActivityService customerActivityService,
            IAddressService addressService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService)
        {
            this._customerService = customerService;
            this._localizationService = localizationService;
            this._EmployeeService = EmployeeService;
            this._permissionService = permissionService;
            this._urlRecordService = urlRecordService;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._pictureService = pictureService;
            this._dateTimeHelper = dateTimeHelper;
            //this._EmployeeSettings = EmployeeSettings;
            this._customerActivityService = customerActivityService;
            this._addressService = addressService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
        }

        #endregion

        #region Utilities

        //[NonAction]
        //protected virtual void UpdatePictureSeoNames(Employee Employee)
        //{
        //    var picture = _pictureService.GetPictureById(Employee.PictureId);
        //    if (picture != null)
        //        _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(Employee.Name));
        //}

        //[NonAction]
        //protected virtual void UpdateLocales(Employee Employee, EmployeeModel model)
        //{
        //    foreach (var localized in model.Locales)
        //    {
        //        _localizedEntityService.SaveLocalizedValue(Employee,
        //                                                       x => x.Name,
        //                                                       localized.Name,
        //                                                       localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(Employee,
        //                                                   x => x.Description,
        //                                                   localized.Description,
        //                                                   localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(Employee,
        //                                                   x => x.MetaKeywords,
        //                                                   localized.MetaKeywords,
        //                                                   localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(Employee,
        //                                                   x => x.MetaDescription,
        //                                                   localized.MetaDescription,
        //                                                   localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(Employee,
        //                                                   x => x.MetaTitle,
        //                                                   localized.MetaTitle,
        //                                                   localized.LanguageId);

        //        //search engine name
        //        var seName = Employee.ValidateSeName(localized.SeName, localized.Name, false);
        //        _urlRecordService.SaveSlug(Employee, seName, localized.LanguageId);
        //    }
        //}

        [NonAction]
        protected virtual void PrepareEmployeeModel(EmployeeModel model, Employee Employee, bool excludeProperties, bool prepareEntireAddressModel)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var address = _addressService.GetAddressById(Employee != null ? Employee.AddressId : 0);

            if (Employee != null)
            {
                if (!excludeProperties)
                {
                    if (address != null)
                    {
                        model.Address = address.ToModel();
                    }
                }

                ////associated customer emails
                //model.AssociatedCustomers = _customerService
                //    .GetAllCustomers(vendorId: Employee.Id)
                //    .Select(c => new EmployeeModel.AssociatedCustomerInfo()
                //    {
                //        Id = c.Id,
                //        Email = c.Email
                //    })
                //    .ToList();
            }

            if (prepareEntireAddressModel)
            {
                model.Address.CountryEnabled = true;
                model.Address.StateProvinceEnabled = true;
                model.Address.CityEnabled = true;
                model.Address.StreetAddressEnabled = true;
                model.Address.StreetAddress2Enabled = true;
                model.Address.ZipPostalCodeEnabled = true;
                model.Address.PhoneEnabled = true;
                model.Address.FaxEnabled = true;

                //address
                model.Address.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries(showHidden: true))
                    model.Address.AvailableCountries.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = (address != null && c.Id == address.CountryId) });

                var states = model.Address.CountryId.HasValue ? _stateProvinceService.GetStateProvincesByCountryId(model.Address.CountryId.Value, showHidden: true).ToList() : new List<StateProvince>();
                if (states.Any())
                {
                    foreach (var s in states)
                        model.Address.AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (address != null && s.Id == address.StateProvinceId) });
                }
                else
                    model.Address.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });
            }
        }

        #endregion

        #region Employees

        //list
        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var model = new EmployeeListModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command, EmployeeListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedKendoGridJson();

            var Employees = _EmployeeService.GetAllEmployees(model.SearchName, command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = Employees.Select(x =>
                {
                    var EmployeeModel = x.ToModel();
                    PrepareEmployeeModel(EmployeeModel, x, false, false);
                    return EmployeeModel;
                }),
                Total = Employees.TotalCount,
            };

            return Json(gridModel);
        }

        //create
        public virtual ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();


            var model = new EmployeeModel();
            PrepareEmployeeModel(model, null, false, true);
            //locales
            //AddLocales(_languageService, model.Locales);
            //default values
           
            model.Active = true;
            

            //default value
            model.Active = true;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual ActionResult Create(EmployeeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var Employee = model.ToEntity();
                _EmployeeService.InsertEmployee(Employee);

                //activity log
                _customerActivityService.InsertActivity("AddNewEmployee", _localizationService.GetResource("ActivityLog.AddNewEmployee"), Employee.Id);

                //search engine name
                //model.SeName = Employee.ValidateSeName(model.SeName, Employee.Name, true);
                //_urlRecordService.SaveSlug(Employee, model.SeName, 0);

                //address
                var address = model.Address.ToEntity();
                address.CreatedOnUtc = DateTime.UtcNow;
                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;
                _addressService.InsertAddress(address);
                Employee.AddressId = address.Id;
                _EmployeeService.UpdateEmployee(Employee);

                //locales
                //UpdateLocales(Employee, model);
                //update picture seo file name
                //UpdatePictureSeoNames(Employee);

                SuccessNotification(_localizationService.GetResource("Admin.Employees.Added"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = Employee.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareEmployeeModel(model, null, true, true);
            return View(model);
        }


        //edit
        public virtual ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var Employee = _EmployeeService.GetEmployeeById(id);
            if (Employee == null || Employee.Deleted)
                //No Employee found with the specified id
                return RedirectToAction("List");

            var model = Employee.ToModel();
            PrepareEmployeeModel(model, Employee, false, true);
            //locales
            //AddLocales(_languageService, model.Locales, (locale, languageId) =>
            //{
            //    locale.Name = Employee.GetLocalized(x => x.Name, languageId, false, false);
            //    locale.Description = Employee.GetLocalized(x => x.Description, languageId, false, false);
            //    locale.MetaKeywords = Employee.GetLocalized(x => x.MetaKeywords, languageId, false, false);
            //    locale.MetaDescription = Employee.GetLocalized(x => x.MetaDescription, languageId, false, false);
            //    locale.MetaTitle = Employee.GetLocalized(x => x.MetaTitle, languageId, false, false);
            //    locale.SeName = Employee.GetSeName(languageId, false, false);
            //});

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(EmployeeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var Employee = _EmployeeService.GetEmployeeById(model.Id);
            if (Employee == null || Employee.Deleted)
                //No Employee found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //int prevPictureId = Employee.PictureId;
                Employee = model.ToEntity(Employee);
                _EmployeeService.UpdateEmployee(Employee);

                //activity log
                _customerActivityService.InsertActivity("EditEmployee", _localizationService.GetResource("ActivityLog.EditEmployee"), Employee.Id);

                //search engine name
                //model.SeName = Employee.ValidateSeName(model.SeName, Employee.Name, true);
                //_urlRecordService.SaveSlug(Employee, model.SeName, 0);

                //address
                var address = _addressService.GetAddressById(Employee.AddressId);
                if (address == null)
                {
                    address = model.Address.ToEntity();
                    address.CreatedOnUtc = DateTime.UtcNow;
                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;
                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;

                    _addressService.InsertAddress(address);
                    Employee.AddressId = address.Id;
                    _EmployeeService.UpdateEmployee(Employee);
                }
                else
                {
                    address = model.Address.ToEntity(address);
                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;
                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;

                    _addressService.UpdateAddress(address);
                }


                ////locales
                //UpdateLocales(Employee, model);
                ////delete an old picture (if deleted or updated)
                //if (prevPictureId > 0 && prevPictureId != Employee.PictureId)
                //{
                //    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                //    if (prevPicture != null)
                //        _pictureService.DeletePicture(prevPicture);
                //}
                ////update picture seo file name
                //UpdatePictureSeoNames(Employee);

                SuccessNotification(_localizationService.GetResource("Admin.Employees.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit",  new {id = Employee.Id});
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareEmployeeModel(model, Employee, true, true);

            return View(model);
        }

        //delete
        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var Employee = _EmployeeService.GetEmployeeById(id);
            if (Employee == null)
                //No Employee found with the specified id
                return RedirectToAction("List");

            //clear associated customer references
            //var associatedCustomers = _customerService.GetAllCustomers(employeeId: Employee.Id);
            //foreach (var customer in associatedCustomers)
            //{
            //    customer.EmployeeId = 0;
            //    _customerService.UpdateCustomer(customer);
            //}

            //delete a Employee
            _EmployeeService.DeleteEmployee(Employee);

            //activity log
            _customerActivityService.InsertActivity("DeleteEmployee", _localizationService.GetResource("ActivityLog.DeleteEmployee"), Employee.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Employees.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        

    }
}
