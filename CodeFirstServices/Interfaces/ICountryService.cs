using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<Country> getallcountries();
        Country GetcountrybyId(int id);
        string GetcountrynamebyId(int id);
        int getidbyname(string name);
    }
}
