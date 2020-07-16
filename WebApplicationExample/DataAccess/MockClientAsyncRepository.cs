using LanguageExt;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationExample.Models;
using static WebApplicationExample.Models.Db;


namespace WebApplicationExample.DataAccess
{
    public class MockClientAsyncRepository : IMockClientAsyncRepository
    {

        public Task<Option<Client>> GetByIdAsync(int id)
        {
            Task<Option<Client>> maybeClientTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Client> maybeClient = Clients.SingleOrDefault(x => x.Id == id);
                return maybeClient;
            });
            return maybeClientTask;
        }
    }
}
