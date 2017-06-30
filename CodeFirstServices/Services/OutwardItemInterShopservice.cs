using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;


namespace CodeFirstServices.Services
{
    public class OutwardItemInterShopservice : IOutwardItemInterShopservice
    {
        private readonly IOutwardItemInterShopRepository _OutwardItemInterShopRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OutwardItemInterShopservice(IOutwardItemInterShopRepository OutwardItemInterShopRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardItemInterShopRepository = OutwardItemInterShopRepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateInterShopitem(OutwardItemInterShop outwarditem)
        {
            _OutwardItemInterShopRepository.Add(outwarditem);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardItemInterShop> GetDetailsByOutwardCode(string code)
        {
            var details = _OutwardItemInterShopRepository.GetMany(m => m.OutwardCode == code);
            return details;
        }

        public OutwardItemInterShop GetDetailsByItemCodeandShopCode(string itemcode,string shopcode)
        {
            var details = _OutwardItemInterShopRepository.Get(m => m.ItemCode==itemcode && m.ToShopCode==shopcode);
            return details;
        }


        public void Update(OutwardItemInterShop outwarditem)
        {
            _OutwardItemInterShopRepository.Update(outwarditem);
            _unitOfWork.Commit();
        }



        public void Delete(OutwardItemInterShop outwarditem)
        {
            _OutwardItemInterShopRepository.Delete(outwarditem);
            _unitOfWork.Commit();
        }
    }
}