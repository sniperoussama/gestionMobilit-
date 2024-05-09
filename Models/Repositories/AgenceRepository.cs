using GestionMobilites.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace GestionMobilites.Models.Repositories
{
    public class AgenceRepository : IMyMobiliteRepository<Agence>
    {
        GestionMobilitesDBContext db;

        public AgenceRepository(GestionMobilitesDBContext _db)
        {
            db = _db;
        }

        public void Add(Agence entity)
        {
            db.Agence.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var agence = Find(id);
            db.Agence.Remove(agence);
            db.SaveChanges();
        }

        public Agence Find(int id)
        {
            var agence = db.Agence.Include(b => b.Region).SingleOrDefault(a => a.Id == id);
            return agence;
        }

        public IList<Agence> List()
        {
            return db.Agence.Include(b => b.Region).ToList();
        }

        public List<Agence> ListAgenceByRegion(int codeRegion)
        {
            var listAgence = db.Agence.Where(a => a.Region.Id == codeRegion).ToList();
            return listAgence;
        }

        public void Update(int id, Agence newAgence)
        {
            db.Update(newAgence);
            db.SaveChanges();
        }

        public List<Agence> Search(string term)
        {
            return db.Agence.Where(a => a.LibelleAgence.Contains(term)).ToList();
        }

        public Agence FindByMatricule(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
