using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;

namespace CodeFirstData.EntityRepositories
{
    public class RetailBillRepository : EntityRepositoryBase<RetailBill>, IRetailBillRepository
    {
        public RetailBillRepository(IDBFactory databaseFactory)
               : base(databaseFactory)
        {
        }
    }
}
