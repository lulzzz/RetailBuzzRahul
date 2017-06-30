using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;


namespace CodeFirstServices.Services
{
   public  class DistrictService:IDistrictService
    {
       private readonly IDistrictRepository _districtrepository;
       public DistrictService(IDistrictRepository districtrepository)
       {
           this._districtrepository = districtrepository;
       }
       public IEnumerable<District> getalldistricts()
       {
           var districts = _districtrepository.GetAll();
           return districts;
       }
       public IEnumerable<District> getDistrictbyState(int id)
       {
           var districts = _districtrepository.GetMany(m => m.Stateid == id);
           return districts;
       }
       public string GetDistrictNamebyId(int id)
       {
           var district = _districtrepository.Get(m => m.DistrictId == id);
           return district.DistrictName;
       }
  
    }
}
