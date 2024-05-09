using System;

namespace GestionMobilites.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  Matricule { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Statut { get; set; }
        public Role Role { get; set; }
        public Agence Agence { get; set; }
    }
}
