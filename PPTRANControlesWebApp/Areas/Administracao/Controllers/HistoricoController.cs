using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize]
    public class HistoricoController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly ApplicationContext _context;
        private readonly HistoricoDAL historicoDAL;
        
        public HistoricoController(ApplicationContext context,
            UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;

            _context = context;
            historicoDAL = new HistoricoDAL(context);           
        }

        // GET: Historico
        public ActionResult Index()
        {
            return View();
        }

        // GET: Historico/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Historico/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Historico/Create
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

        // GET: Historico/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Historico/Edit/5
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

        // GET: Historico/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Historico/Delete/5
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