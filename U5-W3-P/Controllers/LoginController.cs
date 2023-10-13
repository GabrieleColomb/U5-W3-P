using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using U5_W3_P.Models;

namespace U5_W3_P.Controllers
{
    public class LoginController : Controller
    {
        private ModelDbContext Db = new ModelDbContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Accesso cliente)
        {
            if (ModelState.IsValid)
            {
                Clienti user = Db.Clienti.Where(u => u.Username == cliente.Username && u.Password == cliente.Password).FirstOrDefault();

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    Session["userId"] = user.IdCliente;

                    if (user.Ruolo == "Admin")
                    {
                        return RedirectToAction("Index", "Admin"); 
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users"); 
                    }
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}