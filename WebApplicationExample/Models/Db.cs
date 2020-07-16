using System.Collections.Generic;

namespace WebApplicationExample.Models 
{
    public static class Db
    {
        public static List<Client> Clients { get; }= new List<Client>{
                    new  Client{Id=1, Name="Jim",  EmployeeId=1},
                    new  Client{Id=2, Name="John", EmployeeId=2},
                    new  Client{Id=3, Name="Kate", EmployeeId=3}
                };

        public static List<Employee> Employees { get; }=  new List<Employee>{
                    new  Employee{Id=1, Name="Jane"},
                    new  Employee{Id=2, Name="Rick"}
                };

    }

}

