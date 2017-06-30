using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public  interface IGenerateItemCodeService
    {

      void createItemCode(GenerateItemCode generateItemCode);
      void updateItemCode(GenerateItemCode generateItemCode);
     GenerateItemCode getbycount(string shortName, int Count);
     GenerateItemCode getbyshortName(string shortName);

   
    }
}
