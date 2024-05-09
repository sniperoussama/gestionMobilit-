using GestionMobilites.Models;
using GestionMobilites.Models.Repositories;
using GestionMobilites.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionMobilites.Controllers
{
    [Authorize]

    public class AgentController : Controller
    {
        private readonly IMyMobiliteRepository<Agent> agentRepository;
        private readonly IMyMobiliteRepository<Region> regionRepository;
        private readonly IMyMobiliteRepository<Agence> agenceRepository;
        private readonly IMyMobiliteRepository<Role> roleRepository;
        private readonly IConfiguration configuration;

        public AgentController(IMyMobiliteRepository<Agent> agentRepository,
            IMyMobiliteRepository<Agence> agenceRepository,
            IMyMobiliteRepository<Region> regionRepository,
            IMyMobiliteRepository<Role> roleRepository,
             IConfiguration configuration)
        {
            this.agentRepository = agentRepository;
            this.agenceRepository = agenceRepository;
            this.regionRepository = regionRepository;
            this.roleRepository = roleRepository;
            this.configuration = configuration;
        }

        // GET: AgentController
        public ActionResult Index()
        {
            var agents = agentRepository.List();
            return View(agents);
        }

        // GET: AgentController/Details/5
        public ActionResult Details(int id)
        {
            var agent = agentRepository.Find(id);
            return View(agent);
        }

        public ActionResult SearchAgence()
        {
            var model = new Agence();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchAgence(Agence model)
        {
            var agent = agenceRepository.FindByMatricule(model.LibelleAgence);

            if (agent == null)
            {
                ViewBag.Message = "Aucun agent avec ce libelle d'agence n'est trouvé";
                return View();
            }
            else
            {
                var viewModel = new MobiliteViewModel
                {
                    RolesNouveau = FillSelectListRole(),
                    AgencesDestination = FillSelectListAgence(),

                    //RegionSourceId = agent.
                    Agent = agentRepository.FindByMatricule(model.LibelleAgence)
                };

                return View("Create", viewModel);
            }
        }

        // GET: AgentController/Create
        public ActionResult Create()
        {
            var model = new AgentViewModel
            {
                Regions = FillSelectListRegion(),
                Roles = FillSelectListRole()
            };

            return View(model);
        }

        // POST: AgentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgentViewModel model)
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

                    var region = regionRepository.Find(model.RegionId);

                    if (model.AgenceId == -1)
                    {
                        ViewBag.Message = "Veuillez sélectionner une agence !";
                        return View(FillList());
                    }

                    var agence = agenceRepository.Find(model.AgenceId);

                    if (model.RoleId == -1)
                    {
                        ViewBag.Message = "Veuillez sélectionner un rôle !";
                        return View(FillList());
                    }

                    var role = roleRepository.Find(model.RoleId);

                    Agent agent = new Agent
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateDebut = model.DateDebut,
                        DateFin = model.DateFin,
                        Statut = "Actif",
                        Matricule = model.Matricule,
                        Agence = agence,
                        Role = role,
                    };

                    agentRepository.Add(agent);

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

        // GET: AgentController/Edit/5
        public ActionResult Edit(int id)
        {
            var agent = agentRepository.Find(id);

            var model = new AgentViewModel
            {
                Id = agent.Id,
                Matricule = agent.Matricule,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                DateDebut = agent.DateDebut,
                DateFin = agent.DateFin,

                Statut = agent.Statut,
                ListStatut = FillSelectListStatut(),

                RoleId = agent.Role.Id,
                Roles = FillSelectListRole(),

                AgenceId = agent.Agence.Id,
                Agences = FillSelectListAgence(),

                RegionId = agent.Agence.Region.Id,
                Regions = FillSelectListRegion()
            };

            return View(model);
        }

        // POST: AgentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AgentViewModel model)
        {
            try
            {
                var agence = agenceRepository.Find(model.AgenceId);

                if (model.RoleId == -1)
                {
                    ViewBag.Message = "Veuillez sélectionner un rôle !";
                    return View(FillList());
                }

                if (model.Statut == "-1")
                {
                    ViewBag.Message = "Veuillez sélectionner un statut !";
                    return View(FillList());
                }

                var role = roleRepository.Find(model.RoleId);

                Agent agent = new Agent
                {
                    Id = model.Id,
                    Matricule = model.Matricule,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Statut = model.Statut,
                    DateDebut = model.DateDebut,
                    DateFin = model.DateFin,
                    Role = role
                };

                agentRepository.Update(id, agent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AgentController/Delete/5
        public ActionResult Delete(int id)
        {
            var agent = agentRepository.Find(id);
            return View(agent);
        }

        // POST: AgentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Agent agent)
        {
            try
            {
                agentRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public List<Statut> FillSelectListStatut()
        {
            List<Statut> listStatut = new List<Statut>();
            listStatut.Insert(0, new Statut { Id = "-1", Valeur = "--- Merci de sélectionner un statut ---" });
            listStatut.Insert(1, new Statut { Id = "Actif", Valeur = "Actif" });
            listStatut.Insert(2, new Statut { Id = "Inactif", Valeur = "Inactif" });

            return listStatut;
        }

        List<Region> FillSelectListRegion()
        {
            var regions = regionRepository.List().ToList();
            regions.Insert(0, new Region { Id = -1, LibelleRegion = "--- Veuillez sélectionner une region ---" });
            return regions;
        }

        List<Agence> FillSelectListAgence()
        {
            var agences = agenceRepository.List().ToList();
            agences.Insert(0, new Agence { Id = -1, LibelleAgence = "--- Veuillez sélectionner une agence ---" });
            return agences;
        }

        List<Role> FillSelectListRole()
        {
            var roles = roleRepository.List().ToList();
            roles.Insert(0, new Role { Id = -1, LibelleRole = "--- Veuillez sélectionner un rôle ---" });
            return roles;
        }

        AgentViewModel FillList()
        {
            var vmodel = new AgentViewModel
            {
                Regions = FillSelectListRegion(),
                Roles = FillSelectListRole(),
                ListStatut = FillSelectListStatut(),
            };

            return vmodel;
        }

        [HttpPost]
        public JsonResult GetListAgenceByCodeRegion(int id)
        {
            List<AgenceReseau> listAgence = FillSelectListAgenceByRegion(id);
            return new JsonResult(listAgence);
        }

        List<AgenceReseau> FillSelectListAgenceByRegion(int id)
        {
            var listDesAgences = agenceRepository.ListAgenceByRegion(id).ToList();

            List<AgenceReseau> listAgence = new List<AgenceReseau>();

            foreach (var agence in listDesAgences)
            {
                listAgence.Add(new AgenceReseau { Id = agence.Id, Name = agence.LibelleAgence });
            }

            return listAgence;
        }
    }
}
