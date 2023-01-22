using MasterCompany.Models;

namespace MasterCompany.Controllers
{
    public static class EmployeesFunctions
    {
        //Function to filter the employees, returns a list of employees where they are not repeated.
        public static List<Employee> FilterDuplicatedEmployees()
        {
            List<Employee> aux = new();
            foreach (Employee emp1 in EmployeeController.employees)
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
    }
}
