﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;

namespace CodeFirstData.EntityRepositories
{
    public class GetCashRowsBySORepository : EntityRepositoryBase<GetCashRowsBySO>,IGetCashRowsBySORepository
    {
        public GetCashRowsBySORepository(IDBFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
