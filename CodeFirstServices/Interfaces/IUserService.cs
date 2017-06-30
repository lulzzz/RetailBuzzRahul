using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IUserService
    {
        void CreateUser(User user);
        User GetLogin(string username, string pass);
        IEnumerable<User> getAllusers();
        User GetUserById(int id);
        User GetUserByName(string name);
        User GetLastUser();
        IEnumerable<User> GetRegisteredUsersByName(string name);
        IEnumerable<User> GetRegisteredUsersByEmail(string email);
        User GetUserByEmail(string email);
    }
}
