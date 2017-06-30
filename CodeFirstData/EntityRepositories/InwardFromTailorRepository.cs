using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class InwardFromTailorRepository : EntityRepositoryBase<InwardFromTailor>, IInwardFromTailorRepository
    {
        public InwardFromTailorRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
