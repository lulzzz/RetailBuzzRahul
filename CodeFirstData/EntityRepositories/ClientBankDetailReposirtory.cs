using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstData.EntityRepositories
{
    public class ClientBankDetailReposirtory:EntityRepositoryBase<ClientBankDetail>,IClientBankDetailRepository
    {
        public ClientBankDetailReposirtory(IDBFactory databaseFactory)
            : base(databaseFactory)
        { 
        }
    }
}
