using LanguageExt;
using System.Threading.Tasks;
using WebApplicationExample.Models;

namespace WebApplicationExample
{
    public interface IMockClientAsyncRepository
    {
        Task<Option<Client>> GetByIdAsync(int id);
    }
}