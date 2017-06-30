using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class InwardFromTailorItemRepository : EntityRepositoryBase<InwardFromTailorItem>, IInwardFromTailorItemRepository
    {
        public InwardFromTailorItemRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
