using GestionMobilites.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GestionMobilites.Models.Repositories
{
    
    public class AgentRepository : IMyMobiliteRepository<Agent>
    {
        GestionMobilitesDBContext db;

        public AgentRepository(GestionMobilitesDBContext _db)
        {
            db = _db;
        }

        public void Add(Agent entity)
        {
            db.Agent.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var agent = Find(id);
            db.Agent.Remove(agent);
            db.SaveChanges();
        }

        public Agent Find(int id)
        {
            var agent = db.Agent.Include(b => b.Agence).Include(b => b.Role).SingleOrDefault(a => a.Id == id);
            return agent;
        }

        public Agent FindByMatricule(string matricule)
        {
            var agent = db.Agent.Include(b => b.Agence).Include(b => b.Role).SingleOrDefault(a => a.Matricule == matricule);
            return agent;
        }

        public IList<Agent> List()
        {
            return db.Agent.Include(b => b.Agence).Include(b => b.Role).ToList();
        }

        public List<Agent> ListAgenceByRegion(int codeRegion)
        {
            throw new System.NotImplementedException();
        }

        public List<Agent> Search(string term)
        {
            return db.Agent.Where(
                a => a.Matricule.Contains(term)
                || a.FirstName.Contains(term)
                || a.LastName.Contains(term)
                || a.Role.LibelleRole.Contains(term)).ToList();
        }

        public void Update(int id, Agent newAgent)
        {
            db.Update(newAgent);
            db.SaveChanges();
        }
    }
}
