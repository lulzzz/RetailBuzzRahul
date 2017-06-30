using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardFromTailorService
    {
        InwardFromTailor GetLastRowrByFinYr(string year);
        void Create(InwardFromTailor inward);
        IEnumerable<InwardFromTailor> GetActiveTailor();
    }
}
