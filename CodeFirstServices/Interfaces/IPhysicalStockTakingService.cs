using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IPhysicalStockTakingService
    {
        PhysicalStockTaking GetLastRow();
        void Create(PhysicalStockTaking stock);
        IEnumerable<PhysicalStockTaking> GetRowsByCode(string Code);
    }
}
