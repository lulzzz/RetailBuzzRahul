using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;


namespace CodeFirstServices.Services
{
    public class CityService: ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CityService(ICityRepository cityRepository, IUnitOfWork unitOfWork)
        {
            this._cityRepository = cityRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<City> getAllCities()
        {
            var city = _cityRepository.GetAll();
            return city;
        }

        public City GetCitybyId(int id)
        {
            var city = _cityRepository.Get(type => type.cityId == id);
            return city;
        }
        public IEnumerable<City> GetCityByState(int id)
        {
            var citylist = _cityRepository.GetMany(city => city.stateId == id);
            return citylist;    
        }
        public string GetCityNamebyId(int id)
        {
            var city = _cityRepository.Get(type => type.cityId == id);
            return city.cityName;
        }

        public int GetCityIdByName(string city)
        {
            int cityid = _cityRepository.Get(c => c.cityName == city).cityId;
            return cityid;
        }
    }
}
