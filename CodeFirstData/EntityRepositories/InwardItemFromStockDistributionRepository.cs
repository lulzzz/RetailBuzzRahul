using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class InwardItemFromStockDistributionRepository : EntityRepositoryBase<InwardItemFromStockDistribution>, IInwardItemFromStockDistributionRepository
    {
        public InwardItemFromStockDistributionRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
