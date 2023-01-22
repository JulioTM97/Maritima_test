using MasterCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using System.Drawing;


namespace MasterCompany.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        static string ActiveEmployees = "./Sources/ActiveEmployees.json";
        static string InactiveEmployees = "./Sources/InactiveEmployees.json";
        public static List<Employee> employees = new();

        /// <summary>
        /// Get all Employees without any filter (includes repeat employees).
        /// </summary>
        /// <returns>Returns the list of employee (repeated included).</returns>
        [HttpGet]
        public async Task<List<Employee>> GetAllEmployees()
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            return employees;
        }

        /// <summary>
        /// Get all Employees without repeat.
        /// </summary>
        /// <returns>Returns the list of employee without repeat.</returns>
        [HttpGet("Filtered")]
        public async Task<List<Employee>> GetEmployeesNoDuplicated()
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            List<Employee> FilteredEmployees = EmployeesFunctions.FilterDuplicatedEmployees();
            return FilteredEmployees;
        }

        /// <summary>
        /// Get all Employees with increase of salary and without repeat.
        /// </summary>
        /// <returns>Returns the list of employee with increase of salary and without repeat.</returns>
        [HttpGet("Increased")]
        public async Task<List<Employee>> GetEmployeesWithIncrease()
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            List<Employee> increasedEmployees = EmployeesFunctions.FilterDuplicatedEmployees();
            foreach (Employee employee in increasedEmployees)
                if (employee.Salary > 100000) employee.Salary *= 1.25;
                else employee.Salary *= 1.30;
            return increasedEmployees;
        }

        /// <summary>
        /// //Show the porcent of male and female employees.
        /// </summary>
        /// <returns>Returns a string with the porcent of male and female employees.</returns>
        [HttpGet("Sex_Statistics")]
        [Produces("application/json")]
        public async Task<string> GetSexStatistics()
        {
            double males = 0;
            double females = 0;
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            List<Employee> FilteredEmployees = EmployeesFunctions.FilterDuplicatedEmployees();
            foreach (Employee employee in FilteredEmployees)
                if (employee.Gender == "M") males++;
                else females++;
            double total = males + females;
            return "[{\"males\":" + (Math.Round((males / total), 2) * 100).ToString() + ",\"females\":" + (Math.Round((females / total), 2) * 100).ToString() + "}]";
        }

        /// <summary>
        /// Get Employees in Range, filter by min and max Salary.
        /// </summary>
        /// <param name="minSalary">Minimal salary to select.</param>
        /// <param name="maxSalary">Maximun salary to select.</param>
        /// <returns>Returns the list of employee in the range.</returns>
        [HttpGet("{minSalary}/{maxSalary}")]
        public async Task<List<Employee>> GetEmployeesInRange(int minSalary, int maxSalary)
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            IEnumerable<Employee> employeesInRange = from employee in employees
                                                     where employee.Salary >= minSalary && employee.Salary <= maxSalary
                                                     select employee;
            return employeesInRange.ToList();
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        /// <returns>Returns the added employee.</returns>
        [HttpPost]
        public async Task<Employee> PostEmployee(Employee employee)
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            employees.Add(employee);
            await JsonFileHelper.SaveJsonFile(ActiveEmployees);
            return employee;
        }

        /// <summary>
        /// Delete an employee from the active list.
        /// </summary>
        /// <param name="document">Number of document of the employee.</param>
        /// <returns>Returns the deleted employee.</returns>
        [HttpDelete("{document}")]
        public async Task<Employee> DeleteEmployee(string document)
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            Employee deletedEmployee = new Employee();
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Document == document)
                {
                    deletedEmployee = employees[i];
                    employees.RemoveAt(i);
                }
            }
            await JsonFileHelper.SaveJsonFile(ActiveEmployees);
            return deletedEmployee;
        }

        /// <summary>
        /// Removes the employee from the active list and adds them to the inactive list.
        /// </summary>
        /// <param name="document">Number of document of the employee.</param>
        /// <returns>Returns the disabled employee.</returns>
        [HttpDelete("Disable/{document}")]
        public async Task<Employee> DisableEmployee(string document)
        {
            Employee disabledEmployee = await DeleteEmployee(document);
            await JsonFileHelper.LoadJsonFile(InactiveEmployees);
            employees.Add(disabledEmployee);
            await JsonFileHelper.SaveJsonFile(InactiveEmployees);
            return disabledEmployee;
        }
    }
}