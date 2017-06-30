using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IShopService
    {
        void Create(ShopMaster shop);
        ShopMaster GetById(int id);
        IEnumerable<ShopMaster> GetAll();
        ShopMaster GetShopByCode(string code);
        ShopMaster GetLastShop();
        void Update(ShopMaster shop);
        void Delete(ShopMaster shop);
        ShopMaster GetShopDetailsByName(string name);
        IEnumerable<ShopMaster> GetShopList(string name);
        IEnumerable<GetUsersEmail> GetUserEmails(string procname);
        IEnumerable<ShopMaster> GetRowsByShopCode(string code);
        IEnumerable<ShopMaster> GetAllShopName(string shopname);
        //IEnumerable<ShopMaster> GetShopByCodevalue(string code);
        ShopMaster GetAddressByArea(string area);
        IEnumerable<ShopMaster> GetAddressList(string GdCode);
        ShopMaster CheckShortCode(string Code);
    }
}
