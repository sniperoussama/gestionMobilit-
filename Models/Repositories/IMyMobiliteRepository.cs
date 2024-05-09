using System.Collections.Generic;

namespace GestionMobilites.Models.Repositories
{

    public interface IMyMobiliteRepository<TEntity>

    {
        IList<TEntity> List();

        List<TEntity> ListAgenceByRegion(int codeRegion);

        TEntity Find(int id);

        TEntity FindByMatricule(string id);

        void Add(TEntity entity);

        void Update(int id, TEntity entity);

        void Delete(int id);

        List<TEntity> Search(string term);

    }
}
