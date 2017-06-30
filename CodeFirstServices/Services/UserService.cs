using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;


namespace CodeFirstServices.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iuserrepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this._iuserrepository = userRepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateUser(User user)
        {
            _iuserrepository.Add(user);
            _unitOfWork.Commit();
        }

        public User GetLogin(string username, string pass)
        {
            Func<User, bool> filter = (User p) => p.UserName == username && p.Password == pass;
            User user = _iuserrepository.Get(filter);
            return user;
        }

        public IEnumerable<User> getAllusers()
        {
            var users = _iuserrepository.GetMany(p => p.UserName != "SuperAdmin");
            return users;
        }

        public User GetUserById(int id)
        {
            var user = _iuserrepository.GetById(id);
            return user;
        }

        public User GetUserByName(string name)
        {
            var data = _iuserrepository.Get(m => m.UserName == name);
            return data;
        }

        public User GetUserByEmail(string email)
        {
            var data = _iuserrepository.Get(m => m.Email == email);
            return data;
        }

        public User GetLastUser()
        {
            var data = _iuserrepository.GetAll().LastOrDefault();
            return data;
        }

        public IEnumerable<User> GetRegisteredUsersByName(string name)
        {
            var list = _iuserrepository.GetMany(m => m.UserName == name);
            return list;
        }

        public IEnumerable<User> GetRegisteredUsersByEmail(string email)
        {
            var list = _iuserrepository.GetMany(m => m.Email == email);
            return list;
        }
    }
}
