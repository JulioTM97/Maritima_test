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

        //Get all Employees (No Filters)
        [HttpGet]
        public async Task<List<Employee>> GetAllEmployees()
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            return employees;
        }

        //Get all Employees filtered
        [HttpGet("Filtered")]
        public async Task<List<Employee>> GetEmployeesNoDuplicated()
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            List<Employee> FilteredEmployees = EmployeesFunctions.FilterDuplicatedEmployees();
            return FilteredEmployees;
        }

        //Get all Increase and filtered employees 
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

        //Show the porcent of male and female employees
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

        //Get Employees in Range, filter by min and max Salary
        [HttpGet("{minSalary}/{maxSalary}")]
        public async Task<List<Employee>> GetEmployeesInRange(int minSalary, int maxSalary)
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            IEnumerable<Employee> employeesInRange = from employee in employees
                                                     where employee.Salary >= minSalary && employee.Salary <= maxSalary
                                                     select employee;
            return employeesInRange.ToList();
        }

        //Add a new employee
        [HttpPost]
        public async Task<Employee> PostEmployee(Employee employee)
        {
            await JsonFileHelper.LoadJsonFile(ActiveEmployees);
            employees.Add(employee);
            await JsonFileHelper.SaveJsonFile(ActiveEmployees);
            return employee;
        }

        //Delete an employee
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

        //Removes the employee from the active list and adds them to the inactive list.
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