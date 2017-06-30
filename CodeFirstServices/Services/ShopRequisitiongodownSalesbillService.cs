using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
  public class ShopRequisitiongodownSalesbillService: IShopRequisitionGodownSalesBillService
    {
        private readonly IShopRequisitionGodownSalesBillRepository _ShopRequisitionGodownSalesBillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShopRequisitiongodownSalesbillService(IShopRequisitionGodownSalesBillRepository ShopRequisitionGodownSalesBillRepository, IUnitOfWork unitOfWork)
        {
            this._ShopRequisitionGodownSalesBillRepository = ShopRequisitionGodownSalesBillRepository;
            this._unitOfWork = unitOfWork;
        }

        public ShopRequisitionGodownsalesbill getShopRequisitionDetailsByShopName(string name)
        {
            var value = _ShopRequisitionGodownSalesBillRepository.Get(co => co.FromShopName == name);
            return value;
        }

        public void CreateShopRequisitionGodown(ShopRequisitionGodownsalesbill shoprequisitiongodown)
        {
            _ShopRequisitionGodownSalesBillRepository.Add(shoprequisitiongodown);
            _unitOfWork.Commit();
        }

        public ShopRequisitionGodownsalesbill GetLastShopRequisition()
        {
            var Details = _ShopRequisitionGodownSalesBillRepository.GetAll().LastOrDefault();
            return Details;
        }

        public IEnumerable<ShopRequisitionGodownsalesbill> getAllDetails()
        {
            var value = _ShopRequisitionGodownSalesBillRepository.GetAll();
            return value;
        }

        public IEnumerable<ShopRequisitionGodownsalesbill> getListByPo(string value)
        {
            var list = _ShopRequisitionGodownSalesBillRepository.GetMany(co => co.RequestForPO == "Yes");
            return list;
        }

}
}
