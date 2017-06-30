using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class ShopRequisitionService : IShopRequisitionGodownService
    {
        private readonly IShopRequisitionGodownRepository _shoprequisitionrepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShopRequisitionService(IShopRequisitionGodownRepository shoprequisitionrepository, IUnitOfWork unitOfWork)
        {
            this._shoprequisitionrepository = shoprequisitionrepository;
            this._unitOfWork = unitOfWork;
        }

        public ShopRequisitionGodown GetLastShopRequisition()
        {
            var Details = _shoprequisitionrepository.GetAll().LastOrDefault();
            return Details;
        }

        public void CreateShopRequisitionGodown(ShopRequisitionGodown shoprequisitiongodown)
        {
            _shoprequisitionrepository.Add(shoprequisitiongodown);
            _unitOfWork.Commit();
        }

        public IEnumerable<ShopRequisitionGodown> getAllDetails()
        {
            var value = _shoprequisitionrepository.GetAll();
            return value;
        }


        public ShopRequisitionGodown getShopRequisitionDetailsByShopCode(string code)
        {
            var codevalue = _shoprequisitionrepository.Get(co => co.ReqCode == code);
            return codevalue;
        }


        public IEnumerable<ShopRequisitionGodown> GetREList(string term)
        {
            var data = _shoprequisitionrepository.GetMany(p => p.ReqCode.ToLower().StartsWith(term.ToString().ToLower()));
            return data;
        }
        public ShopRequisitionGodown getShopRequisitionDetailsByShopName(string name)
        {
            var value = _shoprequisitionrepository.Get(co => co.FromShopName == name);
            return value;
        }

        public IEnumerable<ShopRequisitionGodown> GetListByShopCode(string code)
        {
            var list = _shoprequisitionrepository.GetMany(co => co.FromShopCode == code);
            return list;
        }

        public IEnumerable<ShopRequisitionGodown> getListByPo(string value)
        {
            var list=_shoprequisitionrepository.GetMany(co=>co.RequestForPO=="Yes");
            return list;
        }

    }
}
