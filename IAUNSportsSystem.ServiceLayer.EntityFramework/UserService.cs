using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class UserService : IUserService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<User> _users;

        public UserService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Set<User>();
        }

        public async Task<IList<UserModel>> GetAll()
        {
            return await _users.AsNoTracking().Select(u => new UserModel
              {
                  Id = u.Id,
                  FirstName = u.FirstName,
                  LastName = u.LastName,
                  Email = u.Email,
                  Role = u.Role
              }).ToListAsync();
        }

        public void Add(DomainClasses.User user)
        {
            _users.Add(user);
        }

        public void Edit(DomainClasses.User user)
        {
            _users.Attach(user);

            if (!string.IsNullOrEmpty(user.Password))
                _dbContext.Entry(user).Property(u => u.Password).IsModified = true;


            _dbContext.Entry(user).Property(u => u.Email).IsModified = true;
            _dbContext.Entry(user).Property(u => u.FirstName).IsModified = true;
            _dbContext.Entry(user).Property(u => u.LastName).IsModified = true;
            _dbContext.Entry(user).Property(u => u.Role).IsModified = true;

        }

        public void Delete(int userId)
        {
            var user = new User() { Id = userId };
            _users.Attach(user);
            _users.Remove(user);
        }


        public async Task<UserModel> Get(int userId)
        {
            return await _users.Where(u => u.Id == userId).Select(u => new UserModel
            {
                Id = u.Id,
                Role = u.Role,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).FirstOrDefaultAsync();
        }


        public async Task<bool> IsEmailExist(string email)
        {
            return await _users.AnyAsync(u => u.Email == email);
        }


        public async Task<bool> CanChangeEmail(string email, int userId)
        {
            return !await _users.AnyAsync(u => u.Id != userId && u.Email == email);
        }


        public async Task<LoginResult> Login(string email, string password)
        {
            var result = new LoginResult
            {
                IsValid = false,
                UserId = 0
            };

            var user =
                await
                    _users.Where(u => u.Email == email).Select(u => new { u.Id, u.Password })
                        .SingleOrDefaultAsync();

            if (user == null)
            {
                return result;
            }

            if (user.Password == password)
            {
                result.IsValid = true;
                result.UserId = user.Id;
            }
            else
            {
                return result;
            }

            return result;
        }


        public async Task<string> GetFullName(int userId)
        {
            return await _users.Where(u => u.Id == userId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefaultAsync();
        }
    }
}
