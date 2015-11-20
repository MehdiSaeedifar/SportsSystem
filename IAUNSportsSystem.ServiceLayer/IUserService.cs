using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.Models;
using Microsoft.SqlServer.Server;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IUserService
    {
        Task<IList<UserModel>> GetAll();
        void Add(User user);
        void Edit(User user);
        void Delete(int userId);
        Task<UserModel> Get(int userId);
        Task<bool> IsEmailExist(string email);
        Task<bool> CanChangeEmail(string email, int userId);
        Task<LoginResult> Login(string email, string password);
        Task<string> GetFullName(int userId);
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
