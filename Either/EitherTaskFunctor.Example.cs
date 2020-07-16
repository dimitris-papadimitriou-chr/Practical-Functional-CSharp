using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace EitherTaskFunctor.Example
{
 
    public class MockClientRepository
    { 
        public Task<Either<string, Client>> GetByIdAsync(int id)
        {
            Task<Either<string, Client>> maybeClientTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Client> maybeClient = Clients.SingleOrDefault(x => x.Id == id);
                return maybeClient.ToEither("no Client Found");
            });
            return maybeClientTask;
        }

    }

    public class Controller
    {

        MockClientRepository clients = new MockClientRepository();
        public Task<Either<string, string>> GetNameByIdAsync(int clientId)
                => clients.GetByIdAsync(clientId)
                     .MapT(client => client.Name);
        public Task<Either<string, string>> GetNameByIdAsync1(int clientId)
             => clients.GetByIdAsync(clientId)
                  .Map(x => x.Map(client => client.Name));


    }

    public class Demo
    {

        public async Task Run()
        {
            {
                var clientName = (await new Controller().GetNameByIdAsync(3))
                    .Case switch
                {
                    LeftCase<string, string>(var error) => error,
                    RightCase<string, string>(var name) => name,
                    _ => throw new NotImplementedException(),
                };
            }

            {
                var clientName = await new Controller().GetNameByIdAsync(3)
                           .Map(x => x.Case)
                           switch
                {
                    LeftCase<string, string>(var error) => error,
                    RightCase<string, string>(var name) => name,
                    _ => throw new NotImplementedException(),
                };
            }

            { 
                var clientName = (await new Controller().GetNameByIdAsync(3))
                            .Match(
                              Right: (name) => name,
                              Left: (e) => $"error {e}"); 

            }
        }



    }
}