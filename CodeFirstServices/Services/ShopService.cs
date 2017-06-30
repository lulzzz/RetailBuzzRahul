using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetUsersEmailRepository _GetUserEmailRepository; 
        public ShopService(IShopRepository shopRepository, IUnitOfWork unitOfWork,IGetUsersEmailRepository GetUserEmailRepository)
        {
            this._shopRepository = shopRepository;
            this._unitOfWork = unitOfWork;
            this._GetUserEmailRepository=GetUserEmailRepository;
        }

        public void Create(ShopMaster shop)
        {
            _shopRepository.Add(shop);
            _unitOfWork.Commit();
        }

        public ShopMaster GetById(int id)
        {
            var data = _shopRepository.GetById(id);
            return data;
        }

        public IEnumerable<ShopMaster> GetAll()
        {
            var list = _shopRepository.GetMany(m => m.Status == "Active");
            return list;
        }

        public IEnumerable<ShopMaster> GetAllShopName(string shopname)
        {
            var list = _shopRepository.GetMany(m => m.Status == "Active"  && m.ShopName !=shopname);
            return list;
        }

        public ShopMaster GetShopByCode(string code)
        {
            var data = _shopRepository.Get(n => n.ShopCode == code);
            return data;
        }

        //public IEnumerable<ShopMaster> GetShopByCodevalue(string code)
        //{
        //    var data = _shopRepository.GetMany(n => n.ShopCode == code);
        //    return data;
        //}

        public IEnumerable<ShopMaster> GetAddressList(string GdCode)
        {
            var list = _shopRepository.GetMany(l => l.ShopCode == GdCode);
            return list;
        }
        public ShopMaster GetAddressByArea(string area)
        {
            var data = _shopRepository.Get(gd => gd.ShopAddress == area);
            return data;
        }

        public ShopMaster GetLastShop()
        {
            var data = _shopRepository.GetAll().LastOrDefault();
            return data;
        }

        public void Update(ShopMaster shop)
        {
            _shopRepository.Update(shop);
            _unitOfWork.Commit();
        }

        public void Delete(ShopMaster shop)
        {
            _shopRepository.Delete(shop);
            _unitOfWork.Commit();
        }

        public ShopMaster GetShopDetailsByName(string name)
        {
            var details = _shopRepository.Get(s =>s.ShopName == name);
            return details;
        }

        public IEnumerable<ShopMaster> GetShopList(string name)
        {
            var list = _shopRepository.GetMany(l1 => l1.ShopName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.ShopName);
            return list;
        }

        public IEnumerable<GetUsersEmail> GetUserEmails(string procname)
        {
            var data = _GetUserEmailRepository.ExecuteCustomStoredProc(procname);
            return data;
        }

        public IEnumerable<ShopMaster> GetRowsByShopCode(string code)
        {
            var details = _shopRepository.GetMany(s => s.ShopCode == code);
            return details;
        }

        public ShopMaster CheckShortCode(string Code)
        {
            var details = _shopRepository.Get(sh => sh.ShortCode == Code);
            return details;
        }
    }
}
