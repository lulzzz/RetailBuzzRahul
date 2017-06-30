using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Services
{
    public class OutwardItemShopToGodownService : IOutwardItemShopToGodownService
    {
        private readonly IOutwardItemShopToGodownRepository _OutwardItemShopToGodownRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardItemShopToGodownService(IOutwardItemShopToGodownRepository OutwardItemShopToGodownRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardItemShopToGodownRepository = OutwardItemShopToGodownRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CodeFirstEntities.OutwardItemShopToGodown outward)
        {
            _OutwardItemShopToGodownRepository.Add(outward);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardItemShopToGodown> GetItemsById(int Id)
        {
            var details = _OutwardItemShopToGodownRepository.GetMany(m => m.ItemId == Id);
            return details;
        }

        public IEnumerable<OutwardItemShopToGodown> GetItemsByOutwardNo(string OutwardNo)
        {
            var details = _OutwardItemShopToGodownRepository.GetMany(o => o.OutwardCode == OutwardNo);
            return details;
        }


        public void Delete(OutwardItemShopToGodown outwarditem)
        {
            _OutwardItemShopToGodownRepository.Delete(outwarditem);
            _unitOfWork.Commit();
        }
    }
}
