using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardItemInterGodownService
    {
        void Create(OutwardItemInterGodown outward);
        IEnumerable<OutwardItemInterGodown> GetDetailsByOutwardCode(string code);
        void Delete(OutwardItemInterGodown outward);
        OutwardItemInterGodown GetRowByOutwardCode(string OutwardCode);
    }
}
