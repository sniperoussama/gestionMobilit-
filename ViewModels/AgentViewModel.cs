using GestionMobilites.Models;
using System;
using System.Collections.Generic;

namespace GestionMobilites.ViewModels
{
    public class AgentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Matricule { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Statut { get; set; }
        public List<Statut> ListStatut { get; set; }
        public int RoleId { get; set; }
        public List<Role> Roles { get; set; }
        public int AgenceId { get; set; }
        public List<Agence> Agences { get; set; }
        public int RegionId { get; set; }
        public List<Region> Regions { get; set; }

    }
}
