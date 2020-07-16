using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationExample.Models;

namespace WebApplicationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMockClientRepository _clients;
        private readonly IMockEmployeeRepository _employees;

        public HomeController(ILogger<HomeController> logger,
            IMockClientRepository clients,
            IMockEmployeeRepository employees)
        {
            _logger = logger;
            _clients = clients;
            _employees = employees;
        }

        #region MaybeFunctor
        public IActionResult Index() => View();


        [HttpPost]
        public JsonResult GetClinetNameById(int id)
         => Json(_clients.GetById(id)
                          .Map(client => client.Name)
                          .Match(Some: (name) => name,
                                 None: () => "no client"));

        #endregion

        #region MaybeMonad
        public IActionResult MaybeMonad() => View();

        [HttpPost]
        public IActionResult GetAssignedEmployeeNameById(int id) =>
                 Json((from client in _clients.GetById(id)
                       from employee in _employees.GetById(client.EmployeeId)
                       select employee)
                        .Case switch
                 {
                     SomeCase<Employee>(var employee) => employee.Name,
                     NoneCase<Employee> { } => "error",
                 });

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
