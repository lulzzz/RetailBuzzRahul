using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class InwardFromStkDistRepository : EntityRepositoryBase<InwardFromStockDistribution>,IInwardFromStkDistRepository
    {
        public InwardFromStkDistRepository(IDBFactory databaseFactory)
            :base(databaseFactory)
        { 
        
        }
    }
}
