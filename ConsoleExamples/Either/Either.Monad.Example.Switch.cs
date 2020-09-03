using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.Either.Monad.Example.Switch
{

     
    public class MockClientRepository
    { 
        public Either<string, Client> GetById(int id)
        {
            Option<Client> t = Clients.SingleOrDefault(x => x.Id == id);
            return t.ToEither("no Client Found");
        }
    }
     
    public class MockEmployeeRepository
    { 
        public Either<string, Employee> GetById(int id)
        {
            Option<Employee> t = Employees.SingleOrDefault(x => x.Id == id);
            return t.ToEither("no Employee Found");
        }
    }

    public class Controller
    {
        readonly MockEmployeeRepository employees = new MockEmployeeRepository();
        readonly MockClientRepository clients = new MockClientRepository();
        public string GetAssignedEmployeeNameById(int clientId) =>
          clients
                .GetById(clientId)
                .Map(client => client.EmployeeId)
                .Bind(employees.GetById)
                .Case switch
          {
              LeftCase<string, Employee>(var error) => error,
              RightCase<string, Employee>(var employee) => employee.Name,
              _ => throw new NotImplementedException(),
          };

        public string GetAssignedEmployeeNameById1(int clientId) =>
            (from c in clients.GetById(clientId)
             from e in employees.GetById(c.EmployeeId)
             select e)  .Case switch
            {
                LeftCase<string, Employee>(var error) => error,
                RightCase<string, Employee>(var employee) => employee.Name,
                _ => throw new NotImplementedException(),
            };

    }
    public class Demo
    {

        public void Run()
        {
            var assignedEmployee = new Controller().GetAssignedEmployeeNameById1(1);


        }
    }
}

