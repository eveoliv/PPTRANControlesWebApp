using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;

namespace PPTRANControlesWebApp.Controllers
{
    public class EntrevistaController : Controller
    {
        private readonly Context _context;
        private readonly EntrevistaDAL entrevistaDAL;

        public EntrevistaController(Context context)
        {
            _context = context;
            entrevistaDAL = new EntrevistaDAL(context);
        }

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
        public async Task <IActionResult> Edit(long id)
        {
            return await ObterVisaoEntrevistaPorId(id);
        }

        // POST: Formulario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Entrevista entrevista)
        {
            if (id != entrevista.EntrevistaId)
            {
                return NotFound();
            }

            if (entrevista.CPF != null)
            {
                try
                {
                    await entrevistaDAL.GravaEntrevista(entrevista);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();                    
                }

                return RedirectToAction("Index", "Cliente");
            }
            return View(entrevista);
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


        /************************************************************************/
        private async Task<IActionResult> ObterVisaoEntrevistaPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrevista = await entrevistaDAL.ObterEntrevistaPorId((long)id);
            if (entrevista == null)
            {
                return NotFound();
            }

            return View(entrevista);
        }
    }
}