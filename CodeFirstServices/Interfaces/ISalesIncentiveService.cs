using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesIncentiveService
    {
        void CreateSI(SalesIncentiveMaster salesincentive);
        void UpdateSI(SalesIncentiveMaster salesincentive);
        void DeleteSI(SalesIncentiveMaster salesincentive);
        SalesIncentiveMaster GetSIById(int Id);
        IEnumerable<SalesIncentiveMaster> GetSalesIncentiveDetails();
        IEnumerable<SalesIncentiveMaster> GetAllSI();
    }
}
