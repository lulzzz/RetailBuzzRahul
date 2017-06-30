using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
namespace CodeFirstData.EntityRepositories
{
    public class GetItemCodesByQuotAndItemCodeRepository : EntityRepositoryBase<GetItemCodesByQuotAndItemCode>, IGetItemCodesByQuotAndItemCodeRepository
    {
        public GetItemCodesByQuotAndItemCodeRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
