using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DemoMVC.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Person ps)
        {
            String strOutput = "xin Chào" + ps.PersonId + "-" + ps.FullName + "đến từ" + ps.Address;
            ViewBag.Message = strOutput;
            return View();
        }

    }
   
}