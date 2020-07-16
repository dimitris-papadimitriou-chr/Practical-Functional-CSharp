using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.Maybe.Functor.Example1
{
    public class MockClientRepository
    {
        public Option<Client> GetById(int id) => Clients.SingleOrDefault(x => x.Id == id);
    }

    public class Controller
    {
        MockClientRepository clients = new MockClientRepository();
        public Task<string> GetAssignedEmployeeNameById(int clientId) =>
          clients
                .GetById(clientId).AsTask()
                .MapT(client => client.Name)
                .Match(Some: (name) => name,
                       None: () => "there is non employee assigned");
        public string GetNameById(int clientId) =>
       clients
        .GetById(clientId)
        .Map(client => client.Name)
        .Match(Some: (name) => name,
               None: () => "no client");
        public string GetNameById1(int clientId) =>
        clients
        .GetById(clientId)
        .Map(client => client.Name)
         .Case switch
        {
            SomeCase<string>(var name) => name,
            NoneCase<string> { } => "no client",
            _ => throw new NotImplementedException()
        };
        public string GetNameById2(int clientId) =>
        clients
        .GetById(clientId)
        .Map(client => client.Name)
         .Fold("", (a, name) =>
         {
             return name;
         });

        public string GetNameById3(int clientId) =>
         (from client in clients.GetById(clientId)
          select client.Name)
                   .Case switch
         {
             SomeCase<string>(var name) => name,
             NoneCase<string> { } => "no client",
             _ => throw new NotImplementedException()
         };
    }


    public class Demo
    {

        public async ValueTask Run()
        {
            var assignedEmployeeName = await new Controller().GetAssignedEmployeeNameById(1);

        }
    }
}

