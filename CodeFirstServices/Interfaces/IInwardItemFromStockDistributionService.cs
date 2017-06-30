using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardItemFromStockDistributionService
    {
        void Create(InwardItemFromStockDistribution item);
        IEnumerable<InwardItemFromStockDistribution> GetItemListByCode(string code);
    }
}
