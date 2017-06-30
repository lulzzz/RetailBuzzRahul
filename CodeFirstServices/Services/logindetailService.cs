using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
  
    public class logindetailService:IlogindetailService
    {
        private readonly IlogindetailRepository _logindetailrepository;
        public logindetailService(IlogindetailRepository logindetailrepository)
        {
            this._logindetailrepository = logindetailrepository;
        }
        public logindetail getusername(string name)
        {
            var logindetails = _logindetailrepository.Get(m=>m.username==name );
            return logindetails;
        }
        public logindetail getid(int id)
        {
            var details = _logindetailrepository.Get(m => m.loginid == id);
            return details;
        }
    }
}
