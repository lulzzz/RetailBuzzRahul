using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;

namespace CodeFirstData.EntityRepositories
{
    public class logindetailRepository:EntityRepositoryBase<logindetail>,IlogindetailRepository
    {
        public logindetailRepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
