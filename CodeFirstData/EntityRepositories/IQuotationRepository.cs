using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;

namespace CodeFirstData.EntityRepositories
{
    public interface IQuotationRepository : IEntityRepository<Quotation>
    {
    }
}
