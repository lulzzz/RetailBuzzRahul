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
    public class RetailBillItemRepository : EntityRepositoryBase<RetailBillItem>, IRetailBillItemRepository
    {
        public RetailBillItemRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
}
