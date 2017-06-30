using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class OutwardInterShopservice : IOutwardInterShopService
    {
        private readonly IOutwardInterShopRepository _OutwardInterShopRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OutwardInterShopservice(IOutwardInterShopRepository OutwardInterShopRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardInterShopRepository = OutwardInterShopRepository;
            this._unitOfWork = unitOfWork;
        }

        public OutwardInterShop GetLastRow()
        {
            var data = _OutwardInterShopRepository.GetAll().LastOrDefault();
            return data;
        }

        public void CreateOutwardInterShop(OutwardInterShop outwardintershop)
        {
            _OutwardInterShopRepository.Add(outwardintershop);
            _unitOfWork.Commit();

        }
        public OutwardInterShop GetDetailsByOutwardId(int id)
        {
            var details = _OutwardInterShopRepository.Get(m => m.OutwardId == id);
            return details;
        }

        public OutwardInterShop GetDetailsByOutwardCode(string OutwardCode)
        {
            var details = _OutwardInterShopRepository.Get(o => o.OutwardCode == OutwardCode);
            return details;
        }

        public IEnumerable<OutwardInterShop> GetOutwardNo(string term, string ShopCode)
        {
            var details = _OutwardInterShopRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.ToShopCode == ShopCode && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public void Update(OutwardInterShop outward)
        {
            _OutwardInterShopRepository.Update(outward);
            _unitOfWork.Commit();
        }


        public IEnumerable<OutwardInterShop> GetOutwardNoForPrint(string term, string ShopCode)
        {
            var details = _OutwardInterShopRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.FromShopCode == ShopCode).OrderBy(m => m.OutwardCode);
            return details;
        }


        public IEnumerable<OutwardInterShop> GetDetailsByDate(DateTime FromDate, DateTime ToDate)
        {
            var value = _OutwardInterShopRepository.GetMany(cl => Convert.ToDateTime(cl.OutwardDate).Date >= FromDate.Date && Convert.ToDateTime(cl.OutwardDate).Date <= ToDate);
            return value;
        }

        public IEnumerable<OutwardInterShop> GetDailyOutwardInterShop()
        {
            var list = _OutwardInterShopRepository.GetMany(s => Convert.ToDateTime(s.OutwardDate).Date == DateTime.Now.Date);
            return list;
        }

        public OutwardInterShop GetLastOutwardByFinYear(string Year,string ShopCode)
        {
            var details = _OutwardInterShopRepository.GetMany(o => o.OutwardCode.Contains(Year) && o.FromShopCode == ShopCode).OrderBy(o => o.OutwardCode).LastOrDefault();
            return details;
        }


        public void Delete(OutwardInterShop outward)
        {
            _OutwardInterShopRepository.Delete(outward);
            _unitOfWork.Commit();
        }
    }
}
