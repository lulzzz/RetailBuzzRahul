using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IEntryStockService
    {
        EntryStockMaster GetLastRow();
        void Create(EntryStockMaster tax);
        EntryStockMaster GetDataByStockCode(string entrystockcode);
        EntryStockMaster GetLastStockByFinYr(string year);
        IEnumerable<EntryStockMaster> GetItemsByStatus();
    }
}
