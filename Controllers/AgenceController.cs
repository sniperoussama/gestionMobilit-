using GestionMobilites.Models;
using GestionMobilites.Models.Repositories;
using GestionMobilites.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GestionMobilites.Controllers
{
    [Authorize]

    public class AgenceController : Controller
    {
        private readonly IMyMobiliteRepository<Agence> AgenceRepository;
        private readonly IMyMobiliteRepository<Region> RegionRepository;

        public AgenceController(IMyMobiliteRepository<Agence> AgenceRepository, IMyMobiliteRepository<Region> RegionRepository)
        {
            this.AgenceRepository = AgenceRepository;
            this.RegionRepository = RegionRepository;
        }

        // GET: AgenceController
        public ActionResult Index()
        {
            var agences = AgenceRepository.List();
            return View(agences);
        }

        // GET: AgenceController/Details/5
        public ActionResult Details(int id)
        {
            var agence = AgenceRepository.Find(id);
            return View(agence);
        }

        // GET: AgenceController/Create
        public ActionResult Create()
        {
            var model = new AgenceViewModel
            {
                Regions = FillSelectListRegion()
            };

            return View(model);
        }

        // POST: AgenceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.RegionId == -1)
                    {
                        ViewBag.Message = "Veuillez sélectionner une région !";
                        return View(FillList());
                    }

                    var region = RegionRepository.Find(model.RegionId);

                    Agence agence = new Agence
                    {
                        Id = model.Id,
                        LibelleAgence = model.LibelleAgence,
                        Adresse = model.Adresse,
                        Region = region,
                        DateOuverture = model.DateOuverture,
                        NumTelFix = model.NumTelFix
                    };

                    AgenceRepository.Add(agence);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "you have to fill all the required fields");

            return View(FillList());
        }

        // GET: AgenceController/Edit/5
        public ActionResult Edit(int id)
        {
            var agence = AgenceRepository.Find(id);

            var model = new AgenceViewModel
            {
                Id=agence.Id,
                LibelleAgence=agence.LibelleAgence, 
                Adresse=agence.Adresse, 
                NumTelFix=agence.NumTelFix,
                DateOuverture=agence.DateOuverture,
                RegionId = agence.Region.Id,
                Regions = FillSelectListRegion()
            };

            return View(model);
        }

        // POST: AgenceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AgenceViewModel model)
        {
            try
            {
                if (model.RegionId == -1)
                {
                    ViewBag.Message = "Veuillez sélectionner une région !";
                    return View(FillList());
                }

                var region = RegionRepository.Find(model.RegionId);
                Agence agence = new Agence
                {
                    Id = model.Id,
                    LibelleAgence = model.LibelleAgence,
                    Adresse = model.Adresse,
                    Region = region,
                    DateOuverture = model.DateOuverture,
                    NumTelFix = model.NumTelFix
                };

                AgenceRepository.Update(id, agence);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AgenceController/Delete/5
        public ActionResult Delete(int id)
        {
            var agence = AgenceRepository.Find(id);
            return View(agence);
        }

        // POST: AgenceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Agence agence)
        {
            try
            {
                AgenceRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Region> FillSelectListRegion()
        {
            var regions = RegionRepository.List().ToList();
            regions.Insert(0, new Region { Id = -1, LibelleRegion = "--- Veuillez sélectionner une région ---" });
            return regions;
        }

        AgenceViewModel FillList()
        {
            var vmodel = new AgenceViewModel
            {
                Regions = FillSelectListRegion()
            };

            return vmodel;
        }

    }
}
