using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstData.EntityRepositories
{
    public class JobWorkOutwardToClientRepository : EntityRepositoryBase<JobWorkOutwardToClient>, IJobWorkOutwardToClientRepository
    {
        public JobWorkOutwardToClientRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
