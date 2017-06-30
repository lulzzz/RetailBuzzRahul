using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IJobWorkItemService
    {
        void Create(JobWorkItem item);
        IEnumerable<JobWorkItem> GetAllItems();
        JobWorkItem GetLastItem();
        IEnumerable<JobWorkItem> GetInsertedRow(int LastItemBefore, int LastItemAfter);
        JobWorkItem GetItem(int LastItemBefore);
    }
}
