using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

 
namespace PracticalCSharp.Maybe.Monad.Example.Switch
{   
    public class MockClientRepository
    { 
        public Option<Client> GetById(int id) => Clients.SingleOrDefault(x => x.Id == id); 
    }
     
    public class MockEmployeeRepository
    { 
        public Option<Employee> GetById(int id) => Employees.SingleOrDefault(x => x.Id == id); 
    }

    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();
     
        public string GetAssignedClientNameById(int clientId) =>
          clients
            .GetById(clientId)
            .Case switch
          {
              SomeCase<Client>(var client) => client.Name,
              NoneCase<Client> { } => "No Client Found",
              _ => throw new NotImplementedException()
          };

        public string GetAssignedEmployeeNameById(int clientId) =>
              clients
                .GetById(clientId)
                .Map(c => c.EmployeeId)
                .Bind(employees.GetById)
                 .Match(
                  Some: (employee) => employee.Name,
                  None: () => $" No Employee Found"
                );
     
        public string GetAssignedEmployeeNameById1(int clientId) =>
              clients
                .GetById(clientId)
                .Map(c => c.EmployeeId)
                .Bind(employees.GetById)
                .Case switch
              {
                  SomeCase<Employee>(var employee) => employee.Name,
                  NoneCase<Employee> { } => "No Employee Found",
                  _ => throw new NotImplementedException()
              };
     
        public string GetAssignedEmployeeNameById2(int clientId) =>
                 (from client in clients.GetById(clientId)
                  from employee in employees.GetById(client.EmployeeId)
                  select employee)
                    .Case switch
                 {
                     SomeCase<Employee>(var employee) => employee.Name,
                     NoneCase<Employee> { } => "No Employee Found",
                     _ => throw new NotImplementedException()
                 };

    }
    public class Demo
    {

        public void Run()
        {
            var assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(2);
             
        }
    }
}

