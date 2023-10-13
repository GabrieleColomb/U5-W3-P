using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U5_W3_P.Models;

namespace U5_W3_P.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext Db = new ModelDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Registrazione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrazione(Clienti clienti)
        {
            clienti.Ruolo = "User";
            if (ModelState.IsValid)
            {
                Db.Clienti.Add(clienti);
                Db.SaveChanges();
            }
            return View();
        }
    }
}
