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
    public class CardChequeHandoverRepository : EntityRepositoryBase<CardChequeHandover>, ICardChequeHandoverRepository
    {
        public CardChequeHandoverRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
