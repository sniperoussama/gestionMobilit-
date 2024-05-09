using GestionMobilites.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GestionMobilites.ViewModels
{
    public class AgenceViewModel
    {
        public int Id { get; set; }
        public string LibelleAgence { get; set; }
        public string Adresse { get; set; }
        public string NumTelFix { get; set; }
        public DateTime DateOuverture { get; set; }
        public int RegionId { get; set; }
        public List<Region> Regions { get; set; }
    }
}
