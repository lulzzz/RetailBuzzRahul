using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;

namespace CodeFirstServices.Services
{
    public class UserCredentialService : IUserCredentialService
    {
        private readonly IUserCredentialRepository _UserCredentialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserCredentialService(IUserCredentialRepository usercredential, IUnitOfWork unitOfWork)
        {
            this._UserCredentialRepository = usercredential;
            this._unitOfWork = unitOfWork;
        }
        public void CreateUserCredential(UserCredential user)
        {
            _UserCredentialRepository.Add(user);
            _unitOfWork.Commit();
        }

        public void UpdateUserCredential(UserCredential user)
        {
            _UserCredentialRepository.Update(user);
            _unitOfWork.Commit();
        }

        public UserCredential getById(int id)
        {
            var idvalue = _UserCredentialRepository.Get(uc => uc.UserId == id);
            return idvalue;
        }

        public IEnumerable<UserCredential> getUserCredentialListById(int id)
        {
            var userCredentials = _UserCredentialRepository.GetMany(uc => uc.UserId == id);
            return userCredentials;
        }

        public IEnumerable<UserCredential> GetUserCredentialsByEmail(string email)
        {
            var usercred = _UserCredentialRepository.GetMany(uc => uc.Email == email);
            return usercred;
        }

        public UserCredential GetByEmail(string email)
        {
            var data = _UserCredentialRepository.Get(m => m.Email == email);
            return data;
        }

        public UserCredential GetDetailsByEmailStatusAndModuleId(string email, int moduleid)
        {
            var data = _UserCredentialRepository.Get(m => m.Email == email && m.ModuleId==moduleid && m.Status.Contains("add"));
            return data;
        }

        public UserCredential GetDetailsByAssignRightsCode(string code)
        {
            var data = _UserCredentialRepository.Get(m => m.AssignRightsCode == code);
            return data;
        }

        public IEnumerable<UserCredential> GetAllUserCredentials()
        {
            var list = _UserCredentialRepository.GetAll();
            return list;
        }

        public UserCredential GetDataByEmailAndComponyModuleId(string email)
        {
            var usercred = _UserCredentialRepository.Get(uc => uc.Email == email && uc.ModuleId == 1);
            return usercred;
        }

        public IEnumerable<UserCredential> GetDataByEmailAndShopGodowmCode(string email)
        {
            var usercred = _UserCredentialRepository.GetMany(uc => uc.Email == email && uc.AssignRightsCode.Contains("SH") || uc.AssignRightsCode.Contains("GD"));
            return usercred;
        }
    }
}
