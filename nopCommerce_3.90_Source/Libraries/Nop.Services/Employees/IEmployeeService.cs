using Nop.Core;
using Nop.Core.Domain.Employees;

namespace Nop.Services.Employees
{
    /// <summary>
    /// Employee service interface
    /// </summary>
    public partial interface IEmployeeService
    {
        /// <summary>
        /// Gets a Employee by Employee identifier
        /// </summary>
        /// <param name="EmployeeId">Employee identifier</param>
        /// <returns>Employee</returns>
        Employee GetEmployeeById(int EmployeeId);

        /// <summary>
        /// Delete a Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        void DeleteEmployee(Employee Employee);

        /// <summary>
        /// Gets all Employees
        /// </summary>
        /// <param name="name">Employee name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Employees</returns>
        IPagedList<Employee> GetAllEmployees(string name = "", 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        void InsertEmployee(Employee Employee);

        /// <summary>
        /// Updates the Employee
        /// </summary>
        /// <param name="Employee">Employee</param>
        void UpdateEmployee(Employee Employee);



        /// <summary>
        /// Gets a Employee note note
        /// </summary>
        /// <param name="EmployeeNoteId">The Employee note identifier</param>
        /// <returns>Employee note</returns>
        //EmployeeNote GetEmployeeNoteById(int EmployeeNoteId);

        /// <summary>
        /// Deletes a Employee note
        /// </summary>
        /// <param name="EmployeeNote">The Employee note</param>
        //void DeleteEmployeeNote(EmployeeNote EmployeeNote);
    }
}