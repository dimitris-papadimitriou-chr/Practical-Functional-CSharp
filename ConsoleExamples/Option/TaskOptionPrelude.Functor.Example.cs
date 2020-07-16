using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Text.Json;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.TaskOptionPrelude.Functor.Example
{
    public interface IMockClientRepository
    {
        Task<Option<Client>> GetByIdAsync(int id);
    }

    public class MockClientRepository : IMockClientRepository
    {
        public Task<Option<Client>> GetByIdAsync(int id)
           => System.Threading.Tasks.Task.Run(() =>
                     {
                         System.Threading.Thread.Sleep(1000);
                         Option<Client> maybeClient = Clients.SingleOrDefault(x => x.Id == id);
                         return maybeClient;
                     });

    }

    public class HttpClientRepository : IMockClientRepository
    {
        public async Task<Option<Client>> GetByIdAsync(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var streamTask = httpClient.GetStreamAsync($"https://myapi/client/{id}");
                var client = await JsonSerializer.DeserializeAsync<Client>(await streamTask);
                return Some(client);
            }
            else
            {
                return None;
            }
        }
    }

    //public class EFClientRepository
    //{
    //    public async Task<Option<Client>> GetByIdAsync(int id)
    //    {
    //        using (var context = new DbContext())
    //        {
    //            var clients = await context.Clients.ToListAsync();
    //            Option<Client> client = clients.SingleOrDefault(x => x.Id == id);
    //            return client;
    //        }
    //    }
    //}

    public class Controller
    {
        readonly IMockClientRepository clients;
        public Controller(IMockClientRepository clients)
        {
            this.clients = clients;
        }

        public Task<string> GetNameByIdAsync(int clientId) =>
            clients
            .GetByIdAsync(clientId)
            .Map(x => x.Map(client => client.Name))
            .Match(Some: (name) => name,
                   None: () => "no client");

    }
    public class Demo
    {

        public async ValueTask Run()
        {
            var assignedEmployeeName = await new Controller(new MockClientRepository()).GetNameByIdAsync(1);

        }
    }
}

