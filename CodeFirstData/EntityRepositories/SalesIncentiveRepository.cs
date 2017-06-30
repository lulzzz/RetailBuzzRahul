using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;


namespace CodeFirstData.EntityRepositories
{
    public class SalesIncentiveRepository:EntityRepositoryBase<SalesIncentiveMaster>,ISalesIncentiveRepository
    {
        public SalesIncentiveRepository(IDBFactory databasefactory)
            : base(databasefactory)
        {
        }
    }
}
