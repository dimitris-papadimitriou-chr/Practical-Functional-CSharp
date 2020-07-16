using LanguageExt;
using System.Threading.Tasks;
using WebApplicationExample.Models;

namespace WebApplicationExample
{
    public interface IMockEmployeeAsyncRepository
    {
        Task<Option<Employee>> GetByIdAsync(int id);
    }
}