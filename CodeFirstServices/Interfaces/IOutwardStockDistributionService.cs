using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardStockDistributionService
    {
        void Create(OutwardStockDistribution data);
        IEnumerable<OutwardStockDistribution> GetOutwardNo(string term, string code);
        OutwardStockDistribution GetOutwardByCode(string code);
        void Update(OutwardStockDistribution data);
        OutwardStockDistribution GetDetailsById(int id);
        IEnumerable<OutwardStockDistribution> GetOutwardStkDisNo(string term);
        IEnumerable<OutwardStockDistribution> GetAllOutwardNos(string no);
    }
}
