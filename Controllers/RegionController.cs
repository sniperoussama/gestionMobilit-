using GestionMobilites.Models;
using GestionMobilites.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionMobilites.Controllers
{
    [Authorize]

    public class RegionController : Controller
    {
        private readonly IMyMobiliteRepository<Region> regionRepository;

        public RegionController(IMyMobiliteRepository<Region> regionRepository )
        {
            
            this.regionRepository = regionRepository;
        }
        // GET: RegionController
        public ActionResult Index()
        {
            var regions = regionRepository.List();
            return View(regions);
        }

        // GET: RegionController/Details/5
        public ActionResult Details(int id)
        {
            var region = regionRepository.Find(id);
            return View(region);
        }

        // GET: RegionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Region region)
        {
            try
            {
                regionRepository.Add(region);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegionController/Edit/5
        public ActionResult Edit(int id)
        {
            var region = regionRepository.Find(id);
            return View(region);
        }

        // POST: RegionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Region region)
        {
            try
            {
                regionRepository.Update(id, region);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegionController/Delete/5
        public ActionResult Delete(int id)
        {
            var region =regionRepository.Find(id);
            return View(region);
        }

        // POST: RegionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Region region)
        {
            try
            {
                regionRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
