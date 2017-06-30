using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardInterGodownItemService
    {
        void Create(InwardInterGodownItem inward);
        IEnumerable<InwardInterGodownItem> GetItemListByInwardCode(string InwardCode);
    }
}
