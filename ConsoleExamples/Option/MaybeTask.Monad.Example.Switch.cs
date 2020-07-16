using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace MaybeTask.Monad.Example.Switch
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
        public Task<Option<Employee>> GetByIdAsync(int id)
        {
            Task<Option<Employee>> maybeEmployeeTask = System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Option<Employee> maybeEmployee = Employees.SingleOrDefault(x => x.Id == id);
                return maybeEmployee;
            });
            return maybeEmployeeTask;
        }
    }

    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();
        public Task<Option<Employee>> GetAssignedEmployeeNameById(int clientId)
           => clients.GetByIdAsync(clientId)
               .BindT(client => employees.GetByIdAsync(client.EmployeeId));

        public Task<Option<Employee>> GetAssignedEmployeeNameById1(int clientId)
          => clients
         .GetByIdAsync(clientId)
         .MapT(client => client.EmployeeId)
         .BindT(employees.GetByIdAsync)
      ;

        public Task<string> GetAssignedEmployeeNameById2(int clientId)
                => clients
               .GetByIdAsync(clientId)
               .MapT(client => client.EmployeeId)
               .BindT(employees.GetByIdAsync)
               .Map(e => e.Case
               switch
                {
                    SomeCase<Employee>(var client) => client.Name,
                    NoneCase<Employee> { } => "No Client Found",
                    _ => throw new NotImplementedException()
                });


        public Task<string> GetAssignedEmployeeNameById3(int clientId)
        => (from c in clients.GetByIdAsync(clientId)
            from e in employees.GetByIdAsync(c.EmployeeId)
            select e)
               .Map(e => e.Case
               switch
               {
                   SomeCase<Employee>(var client) => client.Name,
                   NoneCase<Employee> { } => "No Client Found",
                   _ => throw new NotImplementedException()
               });

        public Task<string> GetAssignedEmployeeNameById4(int clientId) =>
            (from c in clients.GetByIdAsync(clientId)
             from e in employees.GetByIdAsync(c.EmployeeId)
             select e)
          .Map(e => e.Case
          switch
          {
              SomeCase<Employee>(var client) => client.Name,
              NoneCase<Employee> { } => "No Client Found",
              _ => throw new NotImplementedException()
          });

    }

    public class Demo
    {

        public async ValueTask Run()
        {


            var employee = await new Controller().GetAssignedEmployeeNameById(3);

            var name1 = employee.Case
               switch
            {
                SomeCase<Employee>(var client) => client.Name,
                NoneCase<Employee> { } => "No Client Found",
                _ => throw new NotImplementedException()
            };


        }

    }
}

