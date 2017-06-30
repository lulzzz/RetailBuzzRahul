using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IJobWorkerService
    {
        void Create(JobWorker worker);
        IEnumerable<JobWorker> GetEmail(string mail);
        JobWorker GetLastRow();
        JobWorker GetDetailsById(int id);
        IEnumerable<JobWorker> GetAll();
        JobWorker GetDetailsByName(string name);
        IEnumerable<JobWorker> GetJobWorkerNames(string term);
        IEnumerable<JobWorker> GetActiveJobWorkers();
    }
}
