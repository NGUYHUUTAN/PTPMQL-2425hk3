using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers
{
    public class HethongphanphoiControllor : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Hethongphanphoi htpp)
        {
            string strOutput = "-" + htpp.MaHTPP + "-" + htpp.TenHTPP;
            ViewBag.Message = strOutput;
            return View();
        }
    }
}