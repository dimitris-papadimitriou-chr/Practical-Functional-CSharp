using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;


namespace EitherAsync.Monad.Example.Switch
{

    public class MockClientRepository
    {
        public EitherAsync<string, Client> GetById(int id)
        {
            Option<Client> t = Clients.SingleOrDefault(x => x.Id == id);
            return t.AsTask().ToEitherAsync("no Client Found");
        }
    }

    public class MockEmployeeRepository
    {
        public EitherAsync<string, Employee> GetById(int id)
        {
            Option<Employee> t = Employees.SingleOrDefault(x => x.Id == id);
            return t.AsTask().ToEitherAsync("no Employee Found");
        }
    }

    public class Controller
    {
        readonly MockEmployeeRepository employees = new MockEmployeeRepository();
        readonly MockClientRepository clients = new MockClientRepository();
        public Task<Either<string, Employee>> GetAssignedEmployeeNameById(int clientId)
           => clients
               .GetById(clientId)
               .Map(client => client.EmployeeId)
               .Bind(employees.GetById)
               .ToEither();

        public EitherAsync<string, Employee> GetAssignedEmployeeNameById1(int clientId)
       => clients
           .GetById(clientId)
           .Map(client => client.EmployeeId)
           .Bind(employees.GetById);


        public async Task<string> GetAssignedEmployeeNameById2(int clientId)
                       => await clients
                           .GetById(clientId)
                           .Map(client => client.EmployeeId)
                           .Bind(employees.GetById).Case switch
                       {
                           LeftCase<string, Employee>(var error) => error,
                           RightCase<string, Employee>(var employee) => employee.Name,
                           _ => throw new NotImplementedException(),
                       };
    }

    public class Demo
    {

        public async Task Run()
        {

            {
                var employeeName = await new Controller().GetAssignedEmployeeNameById1(1)
                    .Case switch
                {
                    LeftCase<string, Employee>(var error) => error,
                    RightCase<string, Employee>(var employee) => employee.Name,
                    _ => throw new NotImplementedException(),
                };

            }
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

        }

    }
}

