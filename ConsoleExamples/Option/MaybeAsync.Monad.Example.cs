using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.MaybeAsync.Monad.Example
{
    public class MockClientRepository
    {
        public OptionAsync<Client> GetById(int id)
        {
            Option<Client> t = Clients.SingleOrDefault(x => x.Id == id);
            return t.ToAsync();
        }
    }
 
    public class MockEmployeeRepository
    { 
        public OptionAsync<Employee> GetById(int id)
        {
            Option<Employee> t = Employees.SingleOrDefault(x => x.Id == id);
            return t.ToAsync();
        } 
    }

    public class Controller
    {
        MockEmployeeRepository employees = new MockEmployeeRepository();
        MockClientRepository clients = new MockClientRepository();
        public Task<Option<Employee>> GetAssignedEmployeeNameById(int clientId)
           => clients
               .GetById(clientId)
               .Map(client => client.EmployeeId)
               .Bind(employees.GetById)
               .ToOption();

    }

    public class Demo
    { 
        public async ValueTask Run()
        { 
            var employee = await new Controller().GetAssignedEmployeeNameById(3);
            var namem = employee.Match(
                  Some: (employee) => employee.Name,
                  None: () => $"error "
             ); 
        }

    }
}

