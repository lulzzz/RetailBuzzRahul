using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
    public class ItemTruncateRepository : EntityRepositoryBase<ItemTruncate>, IItemTruncateRepository
    {
        public ItemTruncateRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
   {
   }
    }
}
