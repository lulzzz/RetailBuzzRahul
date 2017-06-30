using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
    public class PurchaseReturnRepository : EntityRepositoryBase<PurchaseReturn>, IPurchaseReturnRepository
    {
        public PurchaseReturnRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
