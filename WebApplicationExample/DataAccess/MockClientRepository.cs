using LanguageExt;
using System.Linq;
using WebApplicationExample.Models;
using static WebApplicationExample.Models.Db;


namespace WebApplicationExample.DataAccess
{
    public class MockClientRepository : IMockClientRepository
    {
        public Option<Client> GetById(int id) => Clients.SingleOrDefault(x => x.Id == id);
    }
}
