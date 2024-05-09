using GestionMobilites.Areas.Identity.Data;
using GestionMobilites.Models;
using GestionMobilites.Models.Repositories;
using GestionMobilites.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionMobilites.Controllers
{
    [Authorize]

    public class MobiliteController : Controller
    {
        private readonly IMyMobiliteRepository<Mobilite> mobiliteRepository;
        private readonly IMyMobiliteRepository<Region> regionRepository;
        private readonly IMyMobiliteRepository<Agent> agentRepository;
        private readonly IMyMobiliteRepository<Agence> agenceRepository;
        private readonly IMyMobiliteRepository<Role> roleRepository;
        private readonly UserManager<GestionMobilitesUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration configuration;

        public MobiliteController(IMyMobiliteRepository<Mobilite> mobiliteRepository,
            IMyMobiliteRepository<Region> regionRepository,
            IMyMobiliteRepository<Agent> agentRepository,
            IMyMobiliteRepository<Agence> agenceRepository,
            IMyMobiliteRepository<Role> roleRepository,
            UserManager<GestionMobilitesUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.mobiliteRepository = mobiliteRepository;
            this.regionRepository = regionRepository;

            this.agentRepository = agentRepository;
            this.agenceRepository = agenceRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        // GET: MobilitéController
        public ActionResult Index()
        {
            var mobilites = mobiliteRepository.List();
            return View(mobilites);
        }

        // GET: MobilitéController/Details/5
        public ActionResult Details(int id)
        {
            var mobilite = mobiliteRepository.Find(id);
            return View(mobilite);
        }

        // GET:
        public ActionResult SearchAgent()
        {
            var model = new Agent();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchAgent(Agent model)
        {
            var agent = agentRepository.FindByMatricule(model.Matricule);

            if (agent == null)
            {
                ViewBag.Message = "Aucun agent avec ce matricule n'est trouvé";
                return View();
            }
            else
            {
                var viewModel = new MobiliteViewModel
                {
                    RegionsDestination = FillSelectListRegion(),
                    AgencesDestination = FillSelectListAgence(),
                    RolesNouveau = FillSelectListRole(),
                    AncienRoleId = agent.Role.Id,
                    AgenceSourceId = agent.Agence.Id,
                    RegionSourceId = agent.Agence.Region.Id,
                    RegionsSource = FillSelectListRegion(),
                    AgencesSource = FillSelectListAgence(),
                    RolesAncien = FillSelectListRole(),
                    Agent = agentRepository.FindByMatricule(model.Matricule)
                };

                return View("Create", viewModel);
            }
        }

        // GET: MobilitéController/Create
        public ActionResult Create()
        {
            //var model = new MobiliteViewModel
            //{
            //    RegionsDestination = FillSelectListRegion(),
            //    RolesNouveau = FillSelectListRole(),
            //};

            return View();
        }

        // POST: MobilitéController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MobiliteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.RegionDestinationId == -1)
                    {
                        ViewBag.Message = "Veuillez sélectionner une région destination !";
                        return View(FillListByMatricule(model.Agent.Matricule));
                    }

                    var region = regionRepository.Find(model.RegionDestinationId);

                    if (model.AgenceDestinationId == -1 || model.AgenceDestinationId == 0)
                    {
                        ViewBag.Message = "Veuillez sélectionner une agence destination !";
                        return View(FillListByMatricule(model.Agent.Matricule));
                    }

                    var agence = agenceRepository.Find(model.AgenceDestinationId);

                    if (model.NouveauRoleId == -1)
                    {
                        ViewBag.Message = "Veuillez sélectionner le nouveau rôle !";
                        return View(FillListByMatricule(model.Agent.Matricule));
                    }

                    var role = roleRepository.Find(model.NouveauRoleId);

                    var agent = agentRepository.FindByMatricule(model.Agent.Matricule);

                    //Ajouter une nouvelle mobilité
                    Mobilite mobilite = new Mobilite
                    {
                        Id = model.Id,
                        Agent = agent,
                        AgenceSource = agenceRepository.Find(agent.Agence.Id),
                        RegionSource = regionRepository.Find(agent.Agence.Region.Id),
                        AncienRole = roleRepository.Find(agent.Role.Id),
                        RegionDestination = region,
                        AgenceDestination = agence,
                        NouveauRole = role,
                        DateMouvement = model.DateMouvement,
                        MatriculeUserSaisie = userManager.GetUserId(User),
                    };

                    mobiliteRepository.Add(mobilite);

                    //Modifier l'agent objet de la mobilité
                    agent.Agence = agence;
                    agent.Role = role;

                    agentRepository.Update(agent.Id, agent);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "you have to fill all the required fields");

            return View(FillListByMatricule(model.Agent.Matricule));
        }

        // GET: MobilitéController/Edit/5
        public ActionResult Edit(int id)
        {
            var mobilite = mobiliteRepository.Find(id);


            var agent = agentRepository.Find(mobilite.Agent.Id);
            var region = regionRepository.Find(mobilite.RegionDestination.Id);


            var model = new MobiliteViewModel
            {
                Id = mobilite.Id,
                MatriculeUserSaisie = mobilite.MatriculeUserSaisie,

                Agent = agent,

                AgenceDestinationId = mobilite.AgenceDestination.Id,
                AgencesDestination = FillSelectListAgence(),

                RegionDestinationId = mobilite.RegionDestination.Id,
                RegionsDestination = FillSelectListRegion(),

                AgenceSourceId = mobilite.AgenceSource.Id,
                AgencesSource = FillSelectListAgence(),

                RegionSourceId = mobilite.RegionSource.Id,
                RegionsSource = FillSelectListRegion(),

                AncienRoleId = mobilite.AncienRole.Id,
                RolesAncien = FillSelectListRole(),

                NouveauRoleId = mobilite.NouveauRole.Id,
                RolesNouveau = FillSelectListRole()

            };

            return View(model);
        }

        // POST: MobilitéController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MobiliteViewModel model)
        {
            try
            {
                if (model.RegionDestinationId == -1)
                {
                    ViewBag.Message = "Veuillez sélectionner une région destination !";
                    return View(FillListByMatricule(model.Agent.Matricule));
                }

                var region = regionRepository.Find(model.RegionDestinationId);

                if (model.AgenceDestinationId == -1 || model.AgenceDestinationId == 0)
                {
                    ViewBag.Message = "Veuillez sélectionner une agence destination !";
                    return View(FillListByMatricule(model.Agent.Matricule));
                }

                var agence = agenceRepository.Find(model.AgenceDestinationId);

                if (model.NouveauRoleId == -1 || model.NouveauRoleId == 0)
                {
                    ViewBag.Message = "Veuillez sélectionner le nouveau rôle !";
                    return View(FillListByMatricule(model.Agent.Matricule));
                }

                var role = roleRepository.Find(model.NouveauRoleId);

                var agent = agentRepository.FindByMatricule(model.Agent.Matricule);

                Mobilite oldMobilite = mobiliteRepository.Find(model.Id);

                Mobilite mobilite = new Mobilite
                {
                    Id = model.Id,
                    Agent = agent,
                    AgenceSource = agenceRepository.Find(oldMobilite.AgenceSource.Id),
                    RegionSource = regionRepository.Find(oldMobilite.RegionSource.Id),
                    AncienRole = roleRepository.Find(oldMobilite.AncienRole.Id),
                    RegionDestination = region,
                    AgenceDestination = agence,
                    NouveauRole = role,
                    DateMouvement = model.DateMouvement,
                    MatriculeUserSaisie = userManager.GetUserId(User),
                };

                mobilite.Agent.Agence = agence;
                mobilite.Agent.Role = role;

                mobiliteRepository.Update(mobilite.Id, mobilite);

                //Modifier l'agent objet de la mobilité
                agent.Agence = agence;
                agent.Role = role;
                



                agentRepository.Update(agent.Id, agent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: MobilitéController/Delete/5
        public ActionResult Delete(int id)
        {
            var mobilite = mobiliteRepository.Find(id);
            return View(mobilite);
        }

        // POST: MobilitéController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Mobilite mobilite)
        {
            try
            {
                mobiliteRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Region> FillSelectListRegion()
        {
            var regions = regionRepository.List().ToList();
            regions.Insert(0, new Region { Id = -1, LibelleRegion = "--- Veuillez sélectionner une région ---" });
            return regions;
        }

        AgenceViewModel GetAllRegions()
        {
            var vmodel = new AgenceViewModel
            {
                Regions = FillSelectListRegion()

            };
            return vmodel;
        }

        List<Role> FillSelectListRole()
        {
            var roles = roleRepository.List().ToList();
            roles.Insert(0, new Role { Id = -1, LibelleRole = "--- Veuillez sélectionner un rôle ---" });
            return roles;
        }

        MobiliteViewModel FillListByMatricule(string matricule)
        {
            var agent = agentRepository.FindByMatricule(matricule);

            var vmodel = new MobiliteViewModel
            {
                RegionsDestination = FillSelectListRegion(),
                AgencesDestination = FillSelectListAgence(),
                RolesNouveau = FillSelectListRole(),
                AncienRoleId = agent.Role.Id,
                AgenceSourceId = agent.Agence.Id,
                RegionSourceId = agent.Agence.Region.Id,
                RegionsSource = FillSelectListRegion(),
                AgencesSource = FillSelectListAgence(),
                RolesAncien = FillSelectListRole(),
                Agent = agent
            };

            return vmodel;
        }

        //[HttpPost]
        //public JsonResult GetListAgenceByCodeRegion(int id)
        //{
        //    List<AgenceReseau> listAgence = FillSelectListAgenceByRegion(id);
        //    return new JsonResult(listAgence);
        //}
        //List<AgenceReseau> FillSelectListAgenceByRegion(int id)
        //{
        //    var listDesAgences = agenceRepository.ListAgenceByRegion(id).ToList();

        //    List<AgenceReseau> listAgence = new List<AgenceReseau>();

        //    foreach (var agence in listDesAgences)
        //    {
        //        listAgence.Add(new AgenceReseau { Id = agence.Id, Name = agence.LibelleAgence });
        //    }

        //    return listAgence;
        //}

        List<Agence> FillSelectListAgence()
        {
            var agences = agenceRepository.List().ToList();
            agences.Insert(0, new Agence { Id = -1, LibelleAgence = "--- Veuillez sélectionner une agence ---" });
            return agences;
        }

    }
}
