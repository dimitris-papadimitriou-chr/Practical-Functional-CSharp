using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace MaybeAsync.Mix.Monad.Example.Switch
{ 
    public class MockClientRepository
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
     
    public class MockEmployeeRepository
    { 
        public OptionAsync<Employee> GetByIdAsync(int id)
        {
            Task<Option<Employee>> maybeEmployeeTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Employee> maybeEmployee = Employees.SingleOrDefault(x => x.Id == id);
                return maybeEmployee;
            });
            return maybeEmployeeTask.ToAsync();
        }

    }

    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();

        public OptionAsync<Employee> GetAssignedEmployeeNameById(int clientId)
   => clients
       .GetByIdAsync(clientId)
       .MapT(client => client.EmployeeId)
       .ToAsync()
       .Bind(employees.GetByIdAsync);

        public Task<Option<Employee>> GetAssignedEmployeeNameById1(int clientId)
           => clients
               .GetByIdAsync(clientId)
               .MapT(client => client.EmployeeId)
               .BindT(x => employees.GetByIdAsync(x).ToOption());
         
    }

    public class Demo
    {

        public async ValueTask Run()
        {


            {
                var employee = await new Controller().GetAssignedEmployeeNameById(3).Case
                          switch
                {
                    SomeCase<Employee>(var client) => client.Name,
                    NoneCase<Employee> { } => "No Client Found",
                    _ => throw new NotImplementedException()
                };
            }

            {
                var employee = (await new Controller().GetAssignedEmployeeNameById1(3)).Case
                          switch
                {
                    SomeCase<Employee>(var client) => client.Name,
                    NoneCase<Employee> { } => "No Client Found",
                    _ => throw new NotImplementedException()
                };
            }


        }

    }
}

