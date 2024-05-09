using GestionMobilites.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GestionMobilites.Models.Repositories
{
    public class RegionRepository : IMyMobiliteRepository<Region>
    {
        GestionMobilitesDBContext db;
        public RegionRepository(GestionMobilitesDBContext _db)
        {
            db = _db;
        }

        public void Add(Region entity)
        {
            db.Region.Add(entity);
            db.SaveChanges();
        }
     
        public void Delete(int id)
        {
            var region = Find(id);
            db.Region.Remove(region);
            db.SaveChanges();
        }

        public Region Find(int id)
        {
            var region = db.Region.SingleOrDefault(a => a.Id == id);
            return region;
        }

        public Region FindByMatricule(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Region> List()
        {
            return db.Region.ToList();
        }

        public List<Region> ListAgenceByRegion(int codeRegion)
        {
            throw new System.NotImplementedException();
        }

        public List<Region> Search(string term)
        {
            return db.Region.Where(a => a.LibelleRegion.Contains(term)).ToList();
        }

        public void Update(int id, Region newRegion)
        {
            db.Update(newRegion);
            db.SaveChanges();
        }
    }
}

