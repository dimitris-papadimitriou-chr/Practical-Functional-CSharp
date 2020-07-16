using System.Collections.Generic;

namespace PracticalCSharp.Model 
{
    public static class Db
    {
        public static List<Client> Clients { get; }= new List<Client>{
                    new  Client{Id=1, Name="Jim",  EmployeeId=1},
                    new  Client{Id=2, Name="John", EmployeeId=4}
                };

        public static List<Employee> Employees { get; }=  new List<Employee>{
                    new  Employee{Id=1, Name="Jim"},
                    new  Employee{Id=2, Name="John"}
                };

    }

}

