using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace DemoMVC.Controllers
{   
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }    
        public string Welcome()
            {
                return "This is the Welcome action method";
            }
        
    }

}