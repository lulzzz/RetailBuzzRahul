using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;

namespace CodeFirstData.EntityRepositories
{
    public class ItemCategoryRepository : EntityRepositoryBase<ItemCategory>, IItemCategoryRepository
    {
        public ItemCategoryRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
