using LanguageExt;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationExample.Models;
using static WebApplicationExample.Models.Db;


namespace WebApplicationExample.DataAccess
{
    public class MockEmployeeAsyncRepository : IMockEmployeeAsyncRepository
    {
        public Task<Option<Employee>> GetByIdAsync(int id)
        {
            Task<Option<Employee>> maybeEmployeeTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                Option<Employee> maybeEmployee = Employees.SingleOrDefault(x => x.Id == id);
                return maybeEmployee;
            });
            return maybeEmployeeTask;
        }
    }
}
