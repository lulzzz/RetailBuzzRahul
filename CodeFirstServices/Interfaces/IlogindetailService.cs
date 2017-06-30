using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IlogindetailService
    {
         logindetail getusername(string name);
         logindetail getid(int id);
    }
}
