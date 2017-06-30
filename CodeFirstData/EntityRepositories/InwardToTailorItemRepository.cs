using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class InwardToTailorItemRepository : EntityRepositoryBase<InwardToTailorItem>, IInwardToTailorItemRepository
    {
        public InwardToTailorItemRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
