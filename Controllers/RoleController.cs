using GestionMobilites.Models;
using GestionMobilites.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionMobilites.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private readonly IMyMobiliteRepository<Role> roleRepository;

        public RoleController(IMyMobiliteRepository<Role>roleRepository)
        {
            this.roleRepository = roleRepository;
        }
        // GET: RoleController
        public ActionResult Index()
        {
            var roles = roleRepository.List();
            return View(roles);
        }

        // GET: RoleController/Details/5
        public ActionResult Details(int id)
        {
            var role = roleRepository.Find(id);
            return View(role);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            try
            {
                roleRepository.Add(role);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = roleRepository.Find(id);
            return View(role);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Role role)
        {
            try
            {
                roleRepository.Update(id, role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        public ActionResult Delete(int id)
        {
            var role = roleRepository.Find(id);
            return View(role);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Role role)
        {
            try
            {
                roleRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
