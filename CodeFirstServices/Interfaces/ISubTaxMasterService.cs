using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public interface ISubTaxMasterService
    {
      void CreateSubTax(SubTaxMaster taxmaster);
      void UpdateSubTax(SubTaxMaster tax);
      IEnumerable<SubTaxMaster> getSubTaxMasterDetails(string code);
      SubTaxMaster getDetailsByCity(string city);
      SubTaxMaster GetTaxesByCity(string city, string taxtype);
	   SubTaxMaster getDetailsById(int id);
      SubTaxMaster getLastInserted();
      IEnumerable<SubTaxMaster> getDetailsOnSpecifiedId(int fromid, int toid);
      IEnumerable<SubTaxMaster> getAllTaxesByStates(string vat, string sat, string ot);
    }
}
