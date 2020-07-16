using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;
using PracticalCSharp.Model;
using static PracticalCSharp.Model.Db;

namespace PracticalCSharp.Maybe.Fold
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
        public string GetAssignedEmployeeNameById(int clientId) =>
          clients
                .GetById(clientId)
                .Fold("", (a, employee) => employee.Name);  

    }
    public class Demo
    {

        public void Run()
        { 
            var assignedEmployeeName = new Controller().GetAssignedEmployeeNameById(1); 
        }
    }
}

