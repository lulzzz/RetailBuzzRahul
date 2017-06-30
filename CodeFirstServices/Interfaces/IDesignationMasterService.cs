using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IDesignationMasterService
    {
        void Create(DesignationMaster designation);
        void Update(DesignationMaster designation);
        IEnumerable<DesignationMaster> getAllDesignation();
        DesignationMaster getLastDesignation();
        IEnumerable<DesignationMaster> GetDesignationList(string name);
        IEnumerable<DesignationMaster> GetDesignationName(string name);
        DesignationMaster getById(int id);
        IEnumerable<DesignationMaster> GetDesignations();
        DesignationMaster GetId(int id);
    }
}
