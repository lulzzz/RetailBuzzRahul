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
    public class InwardFromSupplierRepository : EntityRepositoryBase<InwardFromSupplier>, IInwardFromSupplierRepository
    {
        public InwardFromSupplierRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
