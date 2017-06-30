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
    public class InwardInterShopService : IInwardInterShopService
    {
        private readonly IInwardInterShopRepository _InwardInterShopRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InwardInterShopService(IInwardInterShopRepository InwardInterShopRepository, IUnitOfWork unitOfWork)
        {
            this._InwardInterShopRepository = InwardInterShopRepository;
            this._unitOfWork = unitOfWork;
        }

        public InwardInterShop GetLastInward()
        {
            var details = _InwardInterShopRepository.GetAll().LastOrDefault();
            return details;
        }

        public void Create(InwardInterShop inward)
        {
            _InwardInterShopRepository.Add(inward);
            _unitOfWork.Commit();
        }

        public InwardInterShop GetDetailsById(int Id)
        {
            var details = _InwardInterShopRepository.GetById(Id);
            return details;
        }


        public IEnumerable<InwardInterShop> GetInwardNo(string term, string ShopCode)
        {
            var details = _InwardInterShopRepository.GetMany(i => i.InwardCode.ToLower().Contains(term.ToLower()) && i.ToShopCode == ShopCode).OrderBy(i => i.InwardCode);
            return details;
        }


        public InwardInterShop GetDetailsByInwardNo(string InwardNo)
        {
            var details = _InwardInterShopRepository.Get(i => i.InwardCode == InwardNo);
            return details;
        }

        public IEnumerable<InwardInterShop> GetDetailsByDate(DateTime FromDate, DateTime ToDate)
        {
            var details = _InwardInterShopRepository.GetMany(m => Convert.ToDateTime(m.Date) >= FromDate.Date && Convert.ToDateTime(m.Date) <= ToDate.Date);
            return details;
        }

        public IEnumerable<InwardInterShop> GetDailyInwardInterShop()
        {
            var list = _InwardInterShopRepository.GetMany(i => Convert.ToDateTime(i.Date).Date == DateTime.Now.Date);
            return list;
        }


        public InwardInterShop GetLastInwardByFinYear(string Year,string ShopCode)
        {
            var details = _InwardInterShopRepository.GetMany(p => p.InwardCode.Contains(Year) && p.ToShopCode == ShopCode).OrderBy(p => p.InwardCode).LastOrDefault();
            return details;
        }
    }
}
