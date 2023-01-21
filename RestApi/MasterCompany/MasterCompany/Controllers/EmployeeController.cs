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
        static string fileName = "./Sources/ActiveEmployees.json";
        static List<Employee> employees = new();

        [HttpGet]
        public async Task<List<Employee>> GetAllEmployees()
        {
            await LoadJsonFile();
            return employees;
        }

        #region LoadAndSave
        public async Task<int> LoadJsonFile()
        {
            await using FileStream openStream = System.IO.File.OpenRead(fileName);
            employees = await JsonSerializer.DeserializeAsync<List<Employee>>(openStream);
            return 0;
        }

        public async Task<int> SaveJsonFile()
        {
            using FileStream createStream = System.IO.File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, employees);
            await createStream.DisposeAsync();
            return 0;
        }
        #endregion
    }
}