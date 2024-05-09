using Microsoft.VisualBasic;
using System;

namespace GestionMobilites.Models
{
    public class Agence
    {
        public int Id { get; set; }
        public string LibelleAgence { get; set; }
        public string Adresse { get; set; }
        public string NumTelFix { get; set; }
        public DateTime DateOuverture { get; set; }
        public Region Region { get; set; }

    }
}
