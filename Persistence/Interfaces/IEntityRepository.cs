using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IEntityRepository<Entity> where Entity : IDbEntity
    {
        IEnumerable<Entity> GetAll();
        Entity? GetById(int id);
        Task<bool> AddEntity(Entity entity);
        bool RemoveById(int id);

    }
}
