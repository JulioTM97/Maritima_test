using MasterCompany.Models;
using System.Text.Json;

namespace MasterCompany.Controllers
{
    static class JsonFileHelper
    {
        public static async Task<int> LoadJsonFile(string file)
        {
            await using FileStream openStream = File.OpenRead(file);
            EmployeeController.employees = await JsonSerializer.DeserializeAsync<List<Employee>>(openStream);
            return 0;
        }

        public static async Task<int> SaveJsonFile(string file)
        {
            using FileStream createStream = File.Create(file);
            await JsonSerializer.SerializeAsync(createStream, EmployeeController.employees);
            await createStream.DisposeAsync();
            return 0;
        }
    }
}