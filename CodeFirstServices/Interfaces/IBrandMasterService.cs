using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IBrandMasterService
    {
        void Create(BrandMaster brandmaster);
        void Update(BrandMaster brandmaster);
        BrandMaster getLastBrand();
        IEnumerable<BrandMaster> GetBrandList(string name);
        IEnumerable<BrandMaster> GetBrandName(string name);
        BrandMaster getById(int id);
        IEnumerable<BrandMaster> GetBrands();
        BrandMaster GetId(int id);
        IEnumerable<BrandMaster> GetAllBrands();
        BrandMaster brandnamebyname(string brandname);
    }
}
