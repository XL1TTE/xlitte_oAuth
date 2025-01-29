using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EF_Repository
{
    public class UsersRepository
    {
        private readonly ApplicationContext db;
        public UsersRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<bool> AddAsync(User entity)
        {
            try
            {
                await db.Users.AddAsync(entity);
                await db.SaveChangesAsync();
                db.ChangeTracker.Clear();

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
                db.SaveChangesAsync();
                db.ChangeTracker.Clear();

                return true;
            }
            return false;
        }
    
        public async Task<User?> GetByEmailAsync(string email)
        {
            User? user = await db.Users.FirstOrDefaultAsync(o => o.Email == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
