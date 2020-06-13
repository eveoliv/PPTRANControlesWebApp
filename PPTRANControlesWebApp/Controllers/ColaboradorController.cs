using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PPTRANControlesWebApp.Controllers
{
    public class ColaboradorController : Controller
    {

        public static IList<Colaborador> colaboradores = new List<Colaborador>()
        {
            new Colaborador()
            {
                Id = 1,
                Nome = "Bruno Nash",
                CPF = "65465465465",
                Funcao = "Software Engineer"
            },
            new Colaborador()
            {
                Id = 2,
                Nome = "Caesar Vance",
                CPF = "65465465465",
                Funcao = "Pre-Sales Support"

            },
            new Colaborador()
            {
                Id = 3,
                Nome = "Cara Stevens",
                CPF = "65465465465",
                Funcao = "Sales Assistant"

            },
            new Colaborador()
            {
                Id = 4,
                Nome = "Gavin Joyce",
                CPF = "65465465465",
                Funcao = "Accountant"

            },
            new Colaborador()
            {
                Id = 5,
                Nome = "Donna Snider",
                CPF = "65465465465",
                Funcao = "Customer Support"

            }
        };

        // GET: Colaboradores
        public ActionResult Index()
        {
            return View(colaboradores);
        }

        // GET: Colaboradores/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Colaboradores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colaboradores/Create
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

        // GET: Colaboradores/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Colaboradores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Colaboradores/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Colaboradores/Delete/5
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