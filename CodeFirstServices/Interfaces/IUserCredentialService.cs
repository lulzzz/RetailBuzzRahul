using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public   interface IUserCredentialService
    {
      void CreateUserCredential(UserCredential user);
      UserCredential getById(int id);
      UserCredential GetByEmail(string email);
      IEnumerable<UserCredential> getUserCredentialListById(int id);
      void UpdateUserCredential(UserCredential user);
      IEnumerable<UserCredential> GetUserCredentialsByEmail(string email);
      UserCredential GetDetailsByEmailStatusAndModuleId(string email, int moduleid);
      UserCredential GetDetailsByAssignRightsCode(string code);
      IEnumerable<UserCredential> GetAllUserCredentials();
      UserCredential GetDataByEmailAndComponyModuleId(string email);
      IEnumerable<UserCredential> GetDataByEmailAndShopGodowmCode(string email);
    }
}
