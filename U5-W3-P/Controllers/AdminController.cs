using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using U5_W3_P.Models;

namespace U5_W3_P.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ModelDbContext Db = new ModelDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreaPizza()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreaPizza(Prodotto prodotto)
        {
            if (ModelState.IsValid)
            {
                if (prodotto.FotoFile != null && prodotto.FotoFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(prodotto.FotoFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                    prodotto.FotoFile.SaveAs(path);

                    prodotto.Foto = fileName;
                }
                Db.Prodotto.Add(prodotto);
                Db.SaveChanges();
            }
            return View(prodotto);
        }

        [HttpGet]
        public ActionResult ListaOrdini()
        {
            var listaOrdini = Db.Ordini.Where(o => o.StatoOrdine == false).ToList();
            return View(listaOrdini);
        }

        [HttpPost]
        public ActionResult AggiornaOrdine(int id)
        {
            var order = Db.Ordini.Find(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ordine non trovato");
            }
            order.StatoOrdine = true;
            Db.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult StatisticheGiornaliere()
        {
            return View();
        }

        [HttpGet]
        public JsonResult NumeroOrdiniEvasi()
        {
            int numeroOrdiniEvasi = Db.Ordini.Where(o => o.DataOrdine == DateTime.Today).Count();
            return Json(numeroOrdiniEvasi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TotaleIncassato()
        {
            decimal totaleIncassato = (decimal)Db.Ordini.Where(o => o.DataOrdine == DateTime.Today).Sum(o => o.Importo);
            return Json ( totaleIncassato, JsonRequestBehavior.AllowGet);
        }
    }
}