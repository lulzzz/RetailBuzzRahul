using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
   public interface ICityService
    {
        IEnumerable<City> getAllCities();
        City GetCitybyId(int id);
        IEnumerable<City> GetCityByState(int id);
        string GetCityNamebyId(int id);
        int GetCityIdByName(string city);
    }
}
