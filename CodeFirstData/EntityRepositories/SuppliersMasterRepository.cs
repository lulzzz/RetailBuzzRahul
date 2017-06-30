using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
    public class SuppliersMasterRepository:EntityRepositoryBase<SupplierMaster>, ISuppliersMasterRepository
    {
        public SuppliersMasterRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
}
