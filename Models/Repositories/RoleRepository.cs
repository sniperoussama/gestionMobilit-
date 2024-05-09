using GestionMobilites.Data;
using System.Collections.Generic;
using System.Linq;

namespace GestionMobilites.Models.Repositories
{
    public class RoleRepository : IMyMobiliteRepository<Role>
    {
        GestionMobilitesDBContext db;
        public RoleRepository(GestionMobilitesDBContext _db)
        {
            db = _db;
        }

        public void Add(Role entity)
        {
            db.Role.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var role = Find(id);
            db.Role.Remove(role);
            db.SaveChanges();
        }

        public Role Find(int id)
        {
            var role = db.Role.SingleOrDefault(a => a.Id == id);
            return role;
        }

        public Role FindByMatricule(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Role> List()
        {
            return db.Role.ToList();
        }

        public List<Role> ListAgenceByRegion(int codeRegion)
        {
            throw new System.NotImplementedException();
        }

        public List<Role> Search(string term)
        {
            return db.Role.Where(a => a.LibelleRole.Contains(term)).ToList();
        }

        public void Update(int id, Role newRole)
        {
            db.Update(newRole);
            db.SaveChanges();
        }
    }
}
