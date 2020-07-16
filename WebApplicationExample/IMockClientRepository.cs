using LanguageExt;
using WebApplicationExample.Models;

namespace WebApplicationExample
{
    public interface IMockClientRepository
    {
        Option<Client> GetById(int id);
    }


}