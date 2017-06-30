using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierTemporaryCashMemoItemService
    {
        void Create(CashierTemporaryCashMemoItem cash);
        IEnumerable<CashierTemporaryCashMemoItem> GetDetailsByCashierCode(string code);
    }
}
