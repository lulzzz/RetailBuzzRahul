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
    public class OutwardToShopService : IOutwardToShopService
    {
        private readonly IOutwardToShopRepository _OutwardToShopRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardToShopService(IOutwardToShopRepository OutwardToShopRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardToShopRepository = OutwardToShopRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardToShop godown)
        {
            _OutwardToShopRepository.Add(godown);
            _unitOfWork.Commit();
        }

        public OutwardToShop GetLastRow()
        {
            var row = _OutwardToShopRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<OutwardToShop> GetDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var value = _OutwardToShopRepository.GetMany(cl => Convert.ToDateTime(cl.Date).Date >= fromdate.Date && Convert.ToDateTime(cl.Date).Date <= todate.Date);
            return value;
        }

        public IEnumerable<OutwardToShop> GetDetailsByGodownCode(string godowncode)
        {
            var data = _OutwardToShopRepository.GetMany(d => d.GodownCode == godowncode);
            return data;
        }

        public OutwardToShop GetDetailsbyId(int id)
        {
            var Details = _OutwardToShopRepository.Get(type => type.OutwardId == id);
            return Details;
        }

        public IEnumerable<OutwardToShop> GetActiveOutwards()
        {
            var list = _OutwardToShopRepository.GetMany(m => m.Status == "Active");
            return list;
        }

        public OutwardToShop GetDetailsByOutwardCode(string outwardcode)
        {
            var data = _OutwardToShopRepository.Get(m => m.OutwardCode == outwardcode);
            return data;
        }

        public IEnumerable<OutwardToShop> GetOutwardNo(string term, string shopcode)
        {
            var details = _OutwardToShopRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.Status == "Active" && m.ShopCode == shopcode).OrderBy(m => m.OutwardCode);
            return details;
        }

        public void Udpate(OutwardToShop godown)
        {
            _OutwardToShopRepository.Update(godown);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardToShop> GetOutwardNoForPrint(string term, string godowncode)
        {
            var details = _OutwardToShopRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.GodownCode == godowncode).OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToShop> GetDailyOutwardsToShop()
        {
            var list = _OutwardToShopRepository.GetMany(m => Convert.ToDateTime(m.Date).Date == DateTime.Now.Date);
            return list;
        }

        public OutwardToShop GetLastOutwardByFinYr(string year,string GdCode)
        {
            var data = _OutwardToShopRepository.GetMany(o => o.OutwardCode.Contains(year) && o.GodownCode == GdCode).OrderBy(o => o.OutwardCode).LastOrDefault();
            return data;
        }

        public void Delete(OutwardToShop outward)
        {
            _OutwardToShopRepository.Delete(outward);
            _unitOfWork.Commit();
        }
    }
}
