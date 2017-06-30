using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IDesignService
    {
        void CreateDesign(DesignMaster itemdesign);
        void UpdateDesign(DesignMaster itemdesign);
        DesignMaster GetLastDesignRow();
        DesignMaster GetDetailsById(int Id);
        IEnumerable<DesignMaster> GetDesignNamesBySubCat(string type);
        DesignMaster GetById(int id);
        int getDesignsByDesignCode(string code);
        string getNameByCode(string code);
        DesignMaster GetDetailsByCode(string code);
        IEnumerable<DesignMaster> GetAll();
        IEnumerable<DesignMaster> GetDesignName(string term);
        DesignMaster GetNamebyDesignName(string name);
   }
}
