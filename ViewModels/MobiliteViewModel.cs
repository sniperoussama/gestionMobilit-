using GestionMobilites.Models;
using System;
using System.Collections.Generic;

namespace GestionMobilites.ViewModels
{
    public class MobiliteViewModel
    {
        public int Id { get; set; }
        public Agent Agent { get; set; }
        public int RegionSourceId { get; set; }
        public List<Region> RegionsSource { get; set; }
        public int RegionDestinationId { get; set; }
        public List<Region> RegionsDestination { get; set; }
        public int AgenceSourceId { get; set; }
        public List<Agence> AgencesSource { get; set; }
        public int AgenceDestinationId { get; set; }
        public List<Agence> AgencesDestination { get; set; }
        public int AncienRoleId { get; set; }
        public List<Role> RolesAncien { get; set; }
        public int NouveauRoleId { get; set; }
        public List<Role> RolesNouveau { get; set; }
        public string MatriculeUserSaisie { get; set; }
        public DateTime DateMouvement { get; set; }
    
    }
}
