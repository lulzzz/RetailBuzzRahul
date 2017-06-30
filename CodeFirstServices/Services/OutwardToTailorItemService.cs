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
    public class OutwardToTailorItemService:IOutwardToTailorItemService
    {
        private readonly IOutwardToTailorItemRepository _OutwardToTailorItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardToTailorItemService(IOutwardToTailorItemRepository OutwardToTailorItemRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardToTailorItemRepository = OutwardToTailorItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardToTailorItem outward)
        {
            _OutwardToTailorItemRepository.Add(outward);
            _unitOfWork.Commit();
        }

        public void Update(OutwardToTailorItem outward)
        {
            _OutwardToTailorItemRepository.Update(outward);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardToTailorItem> GetRowsByCode(string code)
        {
            var details = _OutwardToTailorItemRepository.GetMany(d => d.OutwardCode == code);
            return details;
        }

        public IEnumerable<OutwardToTailorItem> GetPendingOutwardToTailors(string outwardcode)
        {
            var data = _OutwardToTailorItemRepository.GetMany(d => d.OutwardCode == outwardcode && d.ItemType == "Inventory");
            return data;
        }
    }
}
