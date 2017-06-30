using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class OutwardShopToGodownRepository : EntityRepositoryBase<OutwardShopToGodown>, IOutwardShopToGodownRepository
    {
        public OutwardShopToGodownRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
