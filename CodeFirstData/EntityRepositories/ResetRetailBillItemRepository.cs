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
    public class ResetRetailBillItemRepository : EntityRepositoryBase<ResetRetailBillItem>, IResetRetailBillItemRepository
    {
        public ResetRetailBillItemRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
