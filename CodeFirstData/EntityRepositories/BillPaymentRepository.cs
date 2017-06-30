using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;

namespace CodeFirstData.EntityRepositories
{
    public class BillPaymentRepository:EntityRepositoryBase<BillPayment>,IBillPaymentRepository
    {
        public BillPaymentRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
