using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class DiscountMasterItemService:IDiscountMasterItemService
    {
        private readonly IDiscountMasterItemRepository _DiscountMasterItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DiscountMasterItemService(IDiscountMasterItemRepository DiscountMasterRepository, IUnitOfWork unitOfWork)
        {
            this._DiscountMasterItemRepository = DiscountMasterRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(DiscountMasterItem discount)
        {
            _DiscountMasterItemRepository.Add(discount);
            _unitOfWork.Commit();
        }

        public DiscountMasterItem getLastRow()
        {
            var data = _DiscountMasterItemRepository.GetAll().LastOrDefault();
            return data;
        }

        public DiscountMasterItem GetLastRowrByFinYr(string year)
        {
            var data = _DiscountMasterItemRepository.GetMany(p => p.DiscountCode.Contains(year)).OrderBy(p => p.DiscountCode).LastOrDefault();
            return data;
        }

        public DiscountMasterItem GetDetailsById(int id)
        {
            var data = _DiscountMasterItemRepository.GetById(id);
            return data;
        }

        public IEnumerable<DiscountMasterItem> GetRowsByCode(string code)
        {
            var data = _DiscountMasterItemRepository.GetMany(d => d.DiscountCode == code);
            return data;
        }

        public DiscountMasterItem GetDetailsByItemCodeAndFromDate(string itemcode,DateTime fromdate)
        {
            var data = _DiscountMasterItemRepository.Get(s => Convert.ToDateTime(s.FromDate).Date <= fromdate && Convert.ToDateTime(s.ToDate).Date >= fromdate && s.ItemCode == itemcode);
            return data;
        }

        public DiscountMasterItem GetDailyDiscountByItemCode(string itemcode)
        {
            var data = _DiscountMasterItemRepository.Get(s => s.ItemCode == itemcode && DateTime.Now.Date >= Convert.ToDateTime(s.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(s.ToDate).Date);
            return data;
        }
    }
}
