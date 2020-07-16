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
    public class HomeAsyncController : Controller
    {
        private readonly ILogger<HomeAsyncController> _logger;
        private readonly IMockClientAsyncRepository _clients;
        private readonly IMockEmployeeAsyncRepository _employees;

        public HomeAsyncController(ILogger<HomeAsyncController> logger,
            IMockClientAsyncRepository clients,
            IMockEmployeeAsyncRepository employees)
        {
            _logger = logger;
            _clients = clients;
            _employees = employees;
        }

        #region MaybeFunctor
        public IActionResult Index() => View();


        [HttpPost]
        public async Task<JsonResult> GetClinetNameById(int id)
         => Json(await _clients.GetByIdAsync(id)
                          .MapT(client => client.Name)
                          .Match(Some: (name) => name,
                                 None: () => "no client"));

        #endregion

        #region MaybeMonad
        public IActionResult MaybeMonad() => View();

        [HttpPost]
        public async Task<JsonResult> GetAssignedEmployeeNameById(int id) =>
                Json(_clients
               .GetByIdAsync(id)
               .MapT(client => client.EmployeeId)
               .BindT(_employees.GetByIdAsync)
               .Map(e => e.Case
               switch
               {
                   SomeCase<Employee>(var client) => client.Name,
                   NoneCase<Employee> { } => "No Client Found",
                   _ => throw new NotImplementedException()
               }));


        [HttpPost]
        public async Task<JsonResult> GetAssignedEmployeeNameById1(int id) =>
            Json(await _clients
           .GetByIdAsync(id)
           .ToAsync()
           .Bind(client =>
                _employees.GetByIdAsync(client.EmployeeId)
                   .ToAsync())
           .Case switch
            {
                SomeCase<Employee>(var client) => client.Name,
                NoneCase<Employee> { } => "No Client Found",
                _ => throw new NotImplementedException()
            });

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
