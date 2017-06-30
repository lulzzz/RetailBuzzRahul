using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
     public interface IJobWorkTypeService
    {
         void Create(JobWorkType type);
         JobWorkType GetLastRow();
         IEnumerable<JobWorkType> GetJobWorkTypeList(string name);
         IEnumerable<JobWorkType> GetJobWorkType(string type);
         JobWorkType GetJobWorkTypeById(int id);
         IEnumerable<JobWorkType> GetAll();
    }
}
