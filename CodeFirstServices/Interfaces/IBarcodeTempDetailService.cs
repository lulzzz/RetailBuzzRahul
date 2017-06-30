using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IBarcodeTempDetailService
    {
        void Create(BarcodeTempDetail BarcodeTempDetail);
        BarcodeTempDetail CheckForData(BarcodeTempDetail Data);
    }
}