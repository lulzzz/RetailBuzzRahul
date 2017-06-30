using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;

namespace CodeFirstData.EntityRepositories
{
    public class SupplierBankDetailRepository:EntityRepositoryBase<SupplierBankDetail>,ISupplierBankDetailRepository
    {
        public SupplierBankDetailRepository(IDBFactory databaseFactory)
           : base(databaseFactory)
       {

       }
    }
}
