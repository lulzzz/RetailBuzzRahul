using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardToTailorItemService
    {
        void Create(OutwardToTailorItem outward);
        IEnumerable<OutwardToTailorItem> GetRowsByCode(string code);
        void Update(OutwardToTailorItem outward);
        IEnumerable<OutwardToTailorItem> GetPendingOutwardToTailors(string outwardcode);
    }
}
