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
    public class EntryStockRepository:EntityRepositoryBase<EntryStockMaster>, IEntryStockRepository
    {
        public EntryStockRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
