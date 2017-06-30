using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public  interface IUnitService
    {
        void createunit(UnitMaster unitmaster);
        void updateunit(UnitMaster unitmaster);
        UnitMaster getLastInserted();
        UnitMaster getById(int id);
        IEnumerable<UnitMaster> getallsize();
        UnitMaster getBycode(string code);
        string GetNumberTypeByUnit(string unit);
        UnitMaster GetDetailsByName(string Name);
        IEnumerable<UnitMaster> GetUnitList(string term);
    }
}
