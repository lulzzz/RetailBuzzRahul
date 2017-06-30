using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
   public class TypeOfMaterialRepository : EntityRepositoryBase<TypeOfMaterial>, ITypeOfMaterialRepository
    {
       public TypeOfMaterialRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
    
}
