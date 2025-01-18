using Domain;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EF_Repository
{
    public class UsersRepository : IEntityRepository<User>
    {
        private readonly ApplicationContext db;
        public UsersRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<bool> AddEntity(User entity)
        {
            try
            {
                await db.Users.AddAsync(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.AsEnumerable();
        }

        public User? GetById(int id)
        {
            User? user = db.Users.Find([id]);
            if (user == null)
            {
                return default;
            }
            return user;
        }

        public bool RemoveById(int id)
        {
            User? user = GetById(id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
