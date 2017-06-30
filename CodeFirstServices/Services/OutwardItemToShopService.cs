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
    public class OutwardItemToShopService : IOutwardItemToShopService
    {
        private readonly IOutwardItemToShopRepository _OutwardItemToShopRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardItemToShopService(IOutwardItemToShopRepository OutwardItemToShopRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardItemToShopRepository = OutwardItemToShopRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardItemToShop godown)
        {
            _OutwardItemToShopRepository.Add(godown);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardItemToShop> GetDetailsByOutwardNo(string outno)
        {
            var details = _OutwardItemToShopRepository.GetMany(d => d.OutwardCode == outno);
            return details;
        }

        public OutwardItemToShop GetDetailsByOutwardCode(string code)
        {
            var data = _OutwardItemToShopRepository.Get(d => d.OutwardCode == code);
            return data;
        }

        public void Update(OutwardItemToShop godown)
        {
            _OutwardItemToShopRepository.Update(godown);
            _unitOfWork.Commit();
        }

        public OutwardItemToShop getLastInsertedOutward()
        {
            var data = _OutwardItemToShopRepository.GetAll().LastOrDefault();
            return data;
        }

        public void Delete(OutwardItemToShop outwarditem)
        {
            _OutwardItemToShopRepository.Delete(outwarditem);
            _unitOfWork.Commit();
        }
    }
}
