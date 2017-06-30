using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
   public class ColorCodeRepository : EntityRepositoryBase<ColorCode>, IColorCodeRepository
    {
        public ColorCodeRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
}
