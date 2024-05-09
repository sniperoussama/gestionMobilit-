using Microsoft.VisualBasic;
using System;

namespace GestionMobilites.Models
{
    public class Mobilite
    {
        public int Id { get; set; }
        public Agent Agent { get; set; }
        public Region RegionSource { get; set; }
        public Region RegionDestination { get; set; }
        public Agence AgenceSource { get; set; }
        public Agence AgenceDestination { get; set; }
        public Role AncienRole { get; set; }
        public Role NouveauRole { get; set; }
        public string MatriculeUserSaisie { get; set; }
        public DateTime DateMouvement { get; set; }
    }
}
