using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PPTRANControlesWebApp.Controllers
{
    public class ClienteController : Controller
    {
        private static IList<Cliente> clientes = new List<Cliente>()
        {
            new Cliente()
            {
                Id = 1,
                Nome = "Tiger Nixon",
                CNH = "123456789",
                CPF = "98765432199"
            },
            new Cliente()
            {
                Id = 2,
                Nome = "Garrett Winters",
                CNH = "111222333",
                CPF = "22223333444"
            },
            new Cliente()
            {
                Id = 3,
                Nome = "Ashton Cox",
                CNH = "1112228883",
                CPF = "22223666644"
            },
            new Cliente()
            {
                Id = 4,
                Nome = "Cedric Kelly",
                CNH = "1112228883",
                CPF = "22223666644"
            },
            new Cliente()
            {
                Id = 5,
                Nome = "Airi Satou",
                CNH = "1112228883",
                CPF = "22223666644"
            }
        };


        // GET: Clientes
        public ActionResult Index()
        {
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long id)
        {

            return View(clientes.Where(c => c.Id == id).First());
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}