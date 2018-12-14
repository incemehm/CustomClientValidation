using CustomClientValidation.Models;
using System;
using System.Web.Mvc;

namespace CustomClientValidation.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Home/
        [HttpPost]
        public ActionResult Index(Person model)
        {
            if (ModelState.IsValid)
                Console.WriteLine("There is something wrong!");

            return View(model);
        }
    }
}