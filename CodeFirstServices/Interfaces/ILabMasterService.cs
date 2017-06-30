using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
 public  interface ILabMasterService
    {
     IEnumerable<LabMaster> getAllLabs();
     LabMaster getById(int id);
    // LabMaster GetLastCategory();
     void createLab(LabMaster labmaster);
     void updateLab(LabMaster labmaster);
     LabMaster getLastInserted();
     LabMaster getByLabCode(string code);


    }
}
