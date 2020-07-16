using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.Either.Monad.Example
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
                   .Match(
                         Right: (employee) => employee.Name,
                         Left: (e) => $"error{e}"
                    );

    }
    public class Demo
    {

        public void Run()
        {
            var assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(1);


        }
    }
}

