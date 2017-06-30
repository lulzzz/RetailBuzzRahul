using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class CatalogueRepository : EntityRepositoryBase<Catalogue>, ICatalogueRepository
    {
        public CatalogueRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
