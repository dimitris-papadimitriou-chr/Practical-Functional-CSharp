using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace EitherAsync.Functor.Example
{
     
    public class MockClientRepository
    { 
        public EitherAsync<string, Client> GetByIdAsync(int id)
        {
            Task<Either<string, Client>> maybeClientTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Client> maybeClient = Clients.SingleOrDefault(x => x.Id == id);
                return maybeClient.ToEither("no Client Found");
            });
            return maybeClientTask.ToAsync();
        }

    }

    public class Controller
    {
        readonly MockClientRepository clients = new MockClientRepository();
        public Task<string> GetNameByIdAsync(int clientId)
                => clients.GetByIdAsync(clientId)
                     .Map(client => client.Name)
                     .Match(
                      Right: (name) => name,
                      Left: (e) => $"error {e}");


        public EitherAsync<string, string> GetNameByIdAsync1(int clientId)
                => clients.GetByIdAsync(clientId)
                     .Map(client => client.Name);

        public async Task<string> GetNameByIdAsync2(int clientId)
        => await clients.GetByIdAsync(clientId)
             .Map(client => client.Name).Case switch
        {
            LeftCase<string, string>(var error) => error,
            RightCase<string, string>(var name) => name,
            _ => throw new NotImplementedException(),
        };


    }

    public class Demo
    {

        public async Task Run()
        {
            {
                var clientName = await new Controller().GetNameByIdAsync(3);
            }
            {
                var clientName = await new Controller().GetNameByIdAsync1(3)
                    .Case switch
                {
                    LeftCase<string, string>(var error) => error,
                    RightCase<string, string>(var name) => name,
                    _ => throw new NotImplementedException(),
                };

            }

            {
                var clientName = await new Controller().GetNameByIdAsync2(1); 
            }


        }



    }
}