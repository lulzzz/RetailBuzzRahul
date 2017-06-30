using CodeFirstData.DBInteractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
    public class OutwardItemInterGodownRepository : EntityRepositoryBase<OutwardItemInterGodown>, IOutwardItemInterGodownRepository
    {
        public OutwardItemInterGodownRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
