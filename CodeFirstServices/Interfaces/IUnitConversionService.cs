using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IUnitConversionService
    {
        void Create(UnitConversion UnitConversion);
        UnitConversion GetUnitValue(string fromunit, string tounit);
        IEnumerable<UnitConversion> GetAll();
    }
}
