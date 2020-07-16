using LanguageExt;
using WebApplicationExample.Models;

namespace WebApplicationExample
{
    public interface IMockEmployeeRepository
    {
        Option<Employee> GetById(int id);
    }
}