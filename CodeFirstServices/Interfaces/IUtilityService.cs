using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
   public  interface IUtilityService
    {
       string getName(string name, int length, int value);
       string Encryptdata(string password);
       string Decryptdata(string encryptpwd);
    }
}
