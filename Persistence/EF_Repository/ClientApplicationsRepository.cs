using Domain;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EF_Repository
{
    public class ClientApplicationsRepository
    {
        private readonly ApplicationContext _db;

        public ClientApplicationsRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<bool> AddEntity(ClientApplication entity)
        {
            try
            {
                await _db.ClientApplications.AddAsync(entity);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Scope? GetScopeByName(string name)
        {
            Scope? scope = _db.Scopes.FirstOrDefault(e => e.Name == name);

            if (scope == null)
            {
                return null;
            }
            return scope;
        }
        public IEnumerable<ClientApplication> GetAll()
        {
            return _db.ClientApplications.AsEnumerable();
        }

        public ClientApplication? GetById(int id)
        {
            ClientApplication? app = _db.ClientApplications.Find([id]);

            if(app == null)
            {
                return default;
            }
            return app;
        }

        public bool RemoveById(int id)
        {
            ClientApplication? app = _db.ClientApplications.Find([id]);

            if (app != null)
            {
                _db.ClientApplications.Remove(app);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
