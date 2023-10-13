using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U5_W3_P.Models;

namespace U5_W3_P.Controllers
{
    [Authorize(Roles = "User")]
    public class UsersController : Controller
    {
        private ModelDbContext Db = new ModelDbContext();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pizze()
        {
            var Pizze = Db.Prodotto.ToList();
            return View(Pizze);
        }

        public ActionResult AggiungiAlCarrello()
        {
            var prodotti = Db.Prodotto.ToList();
            return View(prodotti);
        }

        [HttpPost]
        public ActionResult MoveToCart(int id, int quantita)
        {
            if (Session["IdOrdine"] != null)
            {
                var ordine = new DettaglioOrdini
                {
                    Quantità = Convert.ToString(quantita),
                    ProdottoId = id,
                    OrdineId = Convert.ToInt32(Session["IdOrdine"]),
                };
                Db.DettaglioOrdini.Add(ordine);
                Db.SaveChanges();
                return RedirectToAction("Pizze");
            }
            return RedirectToAction("Pizze");
        }

        [HttpGet]
        public ActionResult Ordine()
        {
            return View();
        }

        //Per ordinare cliccare prima il bottone Ordina le tue pizze
        //successivamente mettere quantità della pizza scelta
        //e cliccare aggiungi al carrello e si potrà vedere il proprio ordine andando su Il tuo ordine
        [HttpPost]
        public ActionResult Ordine(Ordini ordine)
        {
            ordine.DataOrdine = DateTime.Today;
            ordine.Importo = 0;
            ordine.ClienteId = Convert.ToInt32(Session["userId"]);
            ordine.StatoOrdine = false;

            if (ModelState.IsValid)
            {
                Db.Ordini.Add(ordine);
                Db.SaveChanges();
                Session["IdOrdine"] = ordine.IdOrdine;
            }
            return RedirectToAction("Pizze");
        }

        [HttpGet]
        public ActionResult RiepilogoOrdine()
        {
            var IdOrdine = Convert.ToInt16(Session["IdOrdine"]);
            Ordini ordine = Db.Ordini.Where(o => o.IdOrdine == IdOrdine).FirstOrDefault();
            decimal prezzoTotale = 0;

            if (ordine != null)
            {
                foreach (var dettaglio in ordine.DettaglioOrdini)
                {
                    decimal prezzoProdotto = (decimal)dettaglio.Prodotto.Prezzo;
                    prezzoTotale += prezzoProdotto * Convert.ToDecimal(dettaglio.Quantità);
                }
            }
            ViewBag.PrezzoTotale = prezzoTotale;
            ordine.Importo = prezzoTotale;
            Db.SaveChanges();
            ViewBag.PrezzoTotale = prezzoTotale;
            return View(ordine.DettaglioOrdini);
        }

        [HttpPost]
        public ActionResult RiepilogoOrdini(Ordini ordine)
        {
            return View();
        }

        public ActionResult EliminaOrdine(int id)
        {
            var ordine = Db.DettaglioOrdini.Find(id);
            Db.DettaglioOrdini.Remove(ordine);
            Db.SaveChanges();
            return RedirectToAction("Pizze");
        }

        public ActionResult InviaOrdine()
        {
            var IdOrdine = Convert.ToInt16(Session["IdOrdine"]);
            Session.Remove("IdOrdine");
            var test = IdOrdine;

            return RedirectToAction("Index", "Users");
        }
    }
}