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
    public class OutwardItemToShopRepository:EntityRepositoryBase<OutwardItemToShop>,IOutwardItemToShopRepository
    {
        public OutwardItemToShopRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
