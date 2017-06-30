using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;


namespace CodeFirstData.EntityRepositories
{
    public class InwardItemsFromSupplierRepository:EntityRepositoryBase<InwardItemsFromSupplier>,IInwardItemsFromSupplierRepository
    {
        public InwardItemsFromSupplierRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
}
