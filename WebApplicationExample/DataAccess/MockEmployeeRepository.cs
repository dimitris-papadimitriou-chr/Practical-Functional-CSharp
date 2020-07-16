using LanguageExt;
using System.Linq;
using WebApplicationExample.Models;
using static WebApplicationExample.Models.Db;


namespace WebApplicationExample.DataAccess
{
    public class MockEmployeeRepository : IMockEmployeeRepository
    {
        public Option<Employee> GetById(int id) => Employees.SingleOrDefault(x => x.Id == id);
    }
}
