using GestionMobilites.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestionMobilites.Models.Repositories
{
    public class MobilitéRepository : IMyMobiliteRepository<Mobilite>
    {
        GestionMobilitesDBContext db;

        public MobilitéRepository(GestionMobilitesDBContext _db)
        {
            db = _db;
        }
        public void Add(Mobilite entity)
        {
            db.Mobilite.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var mobilité = Find(id);
            db.Mobilite.Remove(mobilité);
            db.SaveChanges();
        }

        public Mobilite Find(int id)
        {
            var mobilité = db.Mobilite.AsNoTracking().Include(b => b.RegionSource)
                .Include(b => b.RegionDestination)
                .Include(b => b.AgenceSource)
                .Include(b => b.AgenceDestination)
                .Include(b => b.AncienRole)
                .Include(b => b.NouveauRole)
                .Include(b => b.Agent)
                .SingleOrDefault(a => a.Id == id);
            return mobilité;
        }

        public Mobilite FindByMatricule(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Mobilite> List()
        {
            return db.Mobilite.Include(b => b.RegionSource)
                .Include(b => b.RegionDestination)
                .Include(b => b.AgenceSource)
                .Include(b => b.AgenceDestination)
                .Include(b => b.AncienRole)
                .Include(b => b.NouveauRole)
                .Include(b => b.Agent)
                .ToList();
        }

        public List<Mobilite> ListAgenceByRegion(int codeRegion)
        {
            throw new System.NotImplementedException();
        }

        public List<Mobilite> Search(string term)
        {
            return db.Mobilite.Where(a => a.Agent.Matricule.Contains(term)).ToList();
        }

        public void Update(int id, Mobilite newMobilité)
        {
            db.Update(newMobilité);
            db.SaveChanges();
        }
    }
}
