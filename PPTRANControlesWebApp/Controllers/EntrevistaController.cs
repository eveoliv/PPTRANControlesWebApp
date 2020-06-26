using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PPTRANControlesWebApp.Controllers
{
    public class EntrevistaController : Controller
    {
        // GET: Formulario
        public ActionResult Index()
        {
            return View();
        }

        // GET: Formulario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Formulario/Create
        public ActionResult Create(long id)
        {     
            return View();
        }

        // POST: Formulario/Create
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

        // GET: Formulario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Formulario/Edit/5
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

        // GET: Formulario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Formulario/Delete/5
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