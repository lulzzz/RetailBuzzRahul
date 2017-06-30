using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public interface ITaxService
    {
      void CreateTax(MainTaxMaster taxmaster);
      MainTaxMaster getLastInsertedTax();
      MainTaxMaster getAllStateDetails(string state);
      MainTaxMaster GetById(int id);


    }
}
