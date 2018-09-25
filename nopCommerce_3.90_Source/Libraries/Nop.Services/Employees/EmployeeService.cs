using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Employees;
using Nop.Services.Events;

namespace Nop.Services.Employees
{
    /// <summary>
    /// Employee service
    /// </summary>
    public partial class EmployeeService : IEmployeeService
    {
        #region Fields

        private readonly IRepository<Employee> _EmployeeRepository;
       // private readonly IRepository<EmployeeNote> _EmployeeNoteRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="EmployeeRepository">Employee repository</param>
        /// <param name="EmployeeNoteRepository">Employee note repository</param>
        /// <param name="eventPublisher">Event published</param>
        public EmployeeService(IRepository<Employee> EmployeeRepository,
            
            IEventPublisher eventPublisher)
        {
            this._EmployeeRepository = EmployeeRepository;
            //this._EmployeeNoteRepository = EmployeeNoteRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets a Employee by Employee identifier
        /// </summary>
        /// <param name="EmployeeId">Employee identifier</param>
        /// <returns>Employee</returns>
        public virtual Employee GetEmployeeById(int EmployeeId)
        {
            if (EmployeeId == 0)
                return null;

            return _EmployeeRepository.GetById(EmployeeId);
        }

        /// <summary>
        /// Delete a Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        public virtual void DeleteEmployee(Employee Employee)
        {
            if (Employee == null)
                throw new ArgumentNullException("Employee");

            Employee.Deleted = true;
            UpdateEmployee(Employee);

            //event notification
            _eventPublisher.EntityDeleted(Employee);
        }

        /// <summary>
        /// Gets all Employees
        /// </summary>
        /// <param name="name">Employee name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Employees</returns>
        public virtual IPagedList<Employee> GetAllEmployees(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _EmployeeRepository.Table;
            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);
            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name);

            var Employees = new PagedList<Employee>(query, pageIndex, pageSize);
            return Employees;
        }

        /// <summary>
        /// Inserts a Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        public virtual void InsertEmployee(Employee Employee)
        {
            if (Employee == null)
                throw new ArgumentNullException("Employee");

            _EmployeeRepository.Insert(Employee);

            //event notification
            _eventPublisher.EntityInserted(Employee);
        }

        /// <summary>
        /// Updates the Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        public virtual void UpdateEmployee(Employee Employee)
        {
            if (Employee == null)
                throw new ArgumentNullException("Employee");

            _EmployeeRepository.Update(Employee);

            //event notification
            _eventPublisher.EntityUpdated(Employee);
        }



        /// <summary>
        /// Gets a Employee note note
        /// </summary>
        /// <param name="EmployeeNoteId">The Employee note identifier</param>
        /// <returns>Employee note</returns>
        //public virtual EmployeeNote GetEmployeeNoteById(int EmployeeNoteId)
        //{
        //    if (EmployeeNoteId == 0)
        //        return null;

        //    return _EmployeeNoteRepository.GetById(EmployeeNoteId);
        //}

        ///// <summary>
        ///// Deletes a Employee note
        ///// </summary>
        ///// <param name="EmployeeNote">The Employee note</param>
        //public virtual void DeleteEmployeeNote(EmployeeNote EmployeeNote)
        //{
        //    if (EmployeeNote == null)
        //        throw new ArgumentNullException("EmployeeNote");

        //    _EmployeeNoteRepository.Delete(EmployeeNote);

        //    //event notification
        //    _eventPublisher.EntityDeleted(EmployeeNote);
        //}

        #endregion
    }
}