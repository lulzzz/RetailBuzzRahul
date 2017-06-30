using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IJobWorkStockService
    {
        void Create(JobWorkStock jobworkstockitem);
        void Update(JobWorkStock jobworkstockitem);
        IEnumerable<JobWorkStock> GetRowsByOutwardNo(string outwardno);
        IEnumerable<JobWorkStock> GetReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<JobWorkStock> GetActiveRowsByOutwardNo(string outwardno);
        IEnumerable<JobWorkStock> GetActiveStock();
    }
}
