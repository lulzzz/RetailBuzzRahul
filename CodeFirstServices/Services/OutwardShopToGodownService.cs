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
    public class OutwardShopToGodownService : IOutwardShopToGodownService
    {
        private readonly IOutwardShopToGodownRepository _OutwardShopToGodownRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardShopToGodownService(IOutwardShopToGodownRepository OutwardShopToGodownRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardShopToGodownRepository = OutwardShopToGodownRepository;
            this._unitOfWork = unitOfWork;
        }

        public OutwardShopToGodown GetLastRow()
        {
            var lastrow = _OutwardShopToGodownRepository.GetAll().LastOrDefault();
            return lastrow;
        }


        public void Create(OutwardShopToGodown outward)
        {
            _OutwardShopToGodownRepository.Add(outward);
            _unitOfWork.Commit();
        }

        public OutwardShopToGodown GetDetailsById(int Id)
        {
            var details = _OutwardShopToGodownRepository.GetById(Id);
            return details;
        }

        public IEnumerable<OutwardShopToGodown> GetOutwardNos(string term, string GodownCode)
        {
            var details = _OutwardShopToGodownRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.GodownCode == GodownCode && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public OutwardShopToGodown GetDetailsByOutwardNo(string OutwardNo)
        {
            var details = _OutwardShopToGodownRepository.Get(o => o.OutwardCode == OutwardNo);
            return details;
        }

        public void Update(OutwardShopToGodown outward)
        {
            _OutwardShopToGodownRepository.Update(outward);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardShopToGodown> GetDetailsByDate(DateTime FromDate, DateTime ToDate)
        {
            var details = _OutwardShopToGodownRepository.GetMany(o => Convert.ToDateTime(o.Date).Date >= FromDate && Convert.ToDateTime(o.Date) <= ToDate.Date);
            return details;
        }

        public IEnumerable<OutwardShopToGodown> GetDetailsByShopAndDate(string Shop, DateTime FromDate, DateTime ToDate)
        {
            var details = _OutwardShopToGodownRepository.GetMany(o => o.ShopName == Shop && Convert.ToDateTime(o.Date) >= FromDate.Date && Convert.ToDateTime(o.Date) <= ToDate.Date);
            return details;
        }

        public IEnumerable<OutwardShopToGodown> GetOutwardNoForPrint(string term, string ShopCode)
        {
            var details = _OutwardShopToGodownRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.ShopCode == ShopCode).OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardShopToGodown> GetDailyOutwardsToGodown()
        {
            var list = _OutwardShopToGodownRepository.GetMany(m => (Convert.ToDateTime(m.Date).Date == DateTime.Now.Date));
            return list;
        }


        public OutwardShopToGodown GetLastOutwardByFinYear(string Year, string ShopCode)
        {
            var details = _OutwardShopToGodownRepository.GetMany(o => o.OutwardCode.Contains(Year) && o.ShopCode == ShopCode).OrderBy(o => o.OutwardCode).LastOrDefault();
            return details;
        }

        public void Delete(OutwardShopToGodown outward)
        {
            _OutwardShopToGodownRepository.Delete(outward);
            _unitOfWork.Commit();
        }
    }
}
