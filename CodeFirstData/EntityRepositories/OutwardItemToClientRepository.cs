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
    public class OutwardItemToClientRepository:EntityRepositoryBase<OutwardItemToClient>,IOutwardItemToClientRepository
    {
        public OutwardItemToClientRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
