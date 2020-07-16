using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;


namespace EitherTask.Monad.Example.Switch
{

    public class MockClientRepository
    {
        public Task<Either<string, Client>> GetByIdAsync(int id)
        {
            Task<Either<string, Client>> eitherClientTask = System.Threading.Tasks.Task.Run(() =>
             {
                 System.Threading.Thread.Sleep(1000);
                 Option<Client> maybeClient = Clients.SingleOrDefault(x => x.Id == id);
                 return maybeClient.ToEither("no Client Found"); ;
             });
            return eitherClientTask;
        }
    }

    public class MockEmployeeRepository
    {
        public Task<Either<string, Employee>> GetByIdAsync(int id)
        {
            Task<Either<string, Employee>> eitherEmployeeTask = System.Threading.Tasks.Task.Run(() =>
             {
                 System.Threading.Thread.Sleep(1000);
                 Option<Employee> maybeEmployee = Employees.SingleOrDefault(x => x.Id == id);
                 return maybeEmployee.ToEither("no Employee Found"); ;
             });
            return eitherEmployeeTask;
        }

    }

    public class Controller
    {
        readonly MockEmployeeRepository employees = new MockEmployeeRepository();
        readonly MockClientRepository clients = new MockClientRepository();
        public Task<Either<string, Employee>> GetAssignedEmployeeNameById(int clientId)
           => clients
               .GetByIdAsync(clientId)
               .MapT(client => client.EmployeeId)
               .BindT(employees.GetByIdAsync);

        public Task<Either<string, Employee>> GetAssignedEmployeeNameById1(int clientId)
   => clients
       .GetByIdAsync(clientId)
       .MapT(client => client.EmployeeId)
       .BindT(employees.GetByIdAsync);


        public Task<string> GetAssignedEmployeeNameById2(int clientId) =>
                      (from c in clients.GetByIdAsync(clientId)
                       from e in employees.GetByIdAsync(c.EmployeeId)
                       select e).Map(e => e.Case switch
                        {
                            LeftCase<string, Employee>(var error) => error,
                            RightCase<string, Employee>(var employee) => employee.Name,
                            _ => throw new NotImplementedException(),
                        });



    }

    public class Demo
    {

        public async Task Run()
        {
            var employeeTask = new Controller().GetAssignedEmployeeNameById(1);

            var namem = (await employeeTask).Case switch
            {
                LeftCase<string, Employee>(var error) => error,
                RightCase<string, Employee>(var employee) => employee.Name,
                _ => throw new NotImplementedException(),
            };

            var namem1 = await employeeTask.Map(t => t.Case switch
              {
                  LeftCase<string, Employee>(var error) => error,
                  RightCase<string, Employee>(var employee) => employee.Name,
                  _ => throw new NotImplementedException(),
              });

            var namem3 = await employeeTask.ContinueWith(t => t.Result.Case switch
            {
                LeftCase<string, Employee>(var error) => error,
                RightCase<string, Employee>(var employee) => employee.Name,
                _ => throw new NotImplementedException(),
            });


            var name4 = await new Controller().GetAssignedEmployeeNameById1(2);
        }

    }
}

