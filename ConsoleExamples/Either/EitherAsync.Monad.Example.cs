using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace EitherAsync.Monad.Example
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
    }

    public class Demo
    { 
        public async Task Run()
        { 
            var employee = await new Controller().GetAssignedEmployeeNameById(3);
            var namem = employee.Match(
                  Right: (employee) => employee.Name,
                  Left: (e) => $"error {e}"
             ); 
        }

    }
}

