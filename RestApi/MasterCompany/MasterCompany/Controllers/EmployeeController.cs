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
        static List<Employee> employees = new();

        //Get all Employees (No Filters)
        [HttpGet]
        public async Task<List<Employee>> GetAllEmployees()
        {
            await LoadJsonFile(ActiveEmployees);
            return employees;
        }

        //Get all Employees filtered
        [HttpGet("Filtered")]
        public async Task<List<Employee>> GetEmployeesNoDuplicated()
        {
            await LoadJsonFile(ActiveEmployees);
            List<Employee> FilteredEmployees = this.FilterDuplicatedEmployees();
            return FilteredEmployees;
        }

        //Get all Increase and filtered employees 
        [HttpGet("Increased")]
        public async Task<List<Employee>> GetEmployeesWithIncrease()
        {
            await LoadJsonFile(ActiveEmployees);
            List<Employee> increasedEmployees = this.FilterDuplicatedEmployees();
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
            await LoadJsonFile(ActiveEmployees);
            List<Employee> FilteredEmployees = this.FilterDuplicatedEmployees();
            foreach (Employee employee in FilteredEmployees)
                if (employee.Gender =="M")males++;
                else females++;
            double total = males + females;
            return "[{\"males\":"+(Math.Round((males / total),2)  *100).ToString() + ",\"females\":"+ (Math.Round((females / total),2) * 100).ToString() + "}]";
        }

        //Function to filter the employees, returns a list of employees where they are not repeated.
        public List<Employee> FilterDuplicatedEmployees() {
            List<Employee> aux = new();
            foreach (Employee emp1 in employees)
            {
                bool isRepeat = false;
                foreach (Employee emp2 in aux)
                {
                    if (emp1.Name == emp2.Name && emp1.LastName == emp2.LastName) isRepeat = true;
                }
                if (!isRepeat) aux.Add(emp1);
            }
            return aux;
        }

        //Get Employees in Range, filter by min and max Salary
        [HttpGet("{minSalary}/{maxSalary}")]
        public async Task<List<Employee>> GetEmployeesInRange(int minSalary, int maxSalary)
        {
            await LoadJsonFile(ActiveEmployees);
            IEnumerable<Employee> employeesInRange = from employee in employees
                                                     where employee.Salary >= minSalary && employee.Salary <= maxSalary
                                                     select employee;
            return employeesInRange.ToList();
        }

        //Add a new employee
        [HttpPost]
        public async Task<Employee> PostEmployee(Employee employee)
        {
            await LoadJsonFile(ActiveEmployees);
            employees.Add(employee);
            await SaveJsonFile(ActiveEmployees);
            return employee;
        }

        //Delete an employee
        [HttpDelete("{document}")]
        public async Task<Employee> DeleteEmployee(string document)
        {
            await LoadJsonFile(ActiveEmployees);
            Employee deletedEmployee = new Employee();
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Document == document)
                {
                    deletedEmployee = employees[i];
                    employees.RemoveAt(i);
                }
            }
            await SaveJsonFile(ActiveEmployees);
            return deletedEmployee;
        }

        //Removes the employee from the active list and adds them to the inactive list.
        [HttpDelete("Disable/{document}")]
        public async Task<Employee> DisableEmployee(string document)
        {
            Employee disabledEmployee = await DeleteEmployee(document);
            await LoadJsonFile(InactiveEmployees);
            employees.Add(disabledEmployee);
            await SaveJsonFile(InactiveEmployees);
            return disabledEmployee;
        }
        #region LoadAndSave
        public async Task<int> LoadJsonFile(string file)
        {
            await using FileStream openStream = System.IO.File.OpenRead(file);
            employees = await JsonSerializer.DeserializeAsync<List<Employee>>(openStream);
            return 0;
        }

        public async Task<int> SaveJsonFile(string file)
        {
            using FileStream createStream = System.IO.File.Create(file);
            await JsonSerializer.SerializeAsync(createStream, employees);
            await createStream.DisposeAsync();
            return 0;
        }
        #endregion
    }
}