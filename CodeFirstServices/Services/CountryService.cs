using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
namespace CodeFirstServices.Services
{
    public class CountryService:ICountryService
    {
        private readonly ICountryRepository _countryrepository;
        
        public CountryService(ICountryRepository countryrepository)
        {
            this._countryrepository = countryrepository;
        }
        public IEnumerable<Country> getallcountries()
        {
            var country = _countryrepository.GetAll();
            return country;
        }
        public Country GetcountrybyId(int id)
        {
            var country = _countryrepository.Get(type => type.countryid == id);
                return country;
        }
        public string GetcountrynamebyId(int id)
        {
            var country = _countryrepository.Get(type => type.countryid == id);
            return country.countryName;
        }
        public int getidbyname(string name)
        {
            var id = _countryrepository.Get(m => m.countryName == name);
            return id.countryid;
        }
    }
}
