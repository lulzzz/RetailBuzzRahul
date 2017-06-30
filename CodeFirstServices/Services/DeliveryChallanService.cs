using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class DeliveryChallanService : IDeliveryChallanService
    {
        private readonly IDeliveryChallanRepository _DeliveryChallanRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public DeliveryChallanService(IDeliveryChallanRepository DeliveryChallanRepository, IUnitOfWork UnitOfWork)
        {
            this._DeliveryChallanRepository = DeliveryChallanRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(DeliveryChallan DeliveryChallan)
        {
            _DeliveryChallanRepository.Add(DeliveryChallan);
            _UnitOfWork.Commit();
        }

        public void Update(DeliveryChallan DeliveryChallan)
        {
            _DeliveryChallanRepository.Update(DeliveryChallan);
            _UnitOfWork.Commit();
        }

        public DeliveryChallan GetLastChallanByFinYr(string year, string shopcode)
        {
            var data = _DeliveryChallanRepository.GetMany(d => d.ChallanNo.Contains(year) && d.ShopCode == shopcode).OrderBy(d => d.ChallanNo).LastOrDefault();
            return data;
        }

        public IEnumerable<DeliveryChallan> GetChallanNos(string billno)
        {
            var data = _DeliveryChallanRepository.GetMany(b => b.ChallanNo.ToLower().Contains(billno.ToString().ToLower()) && b.Status == "Active" && b.Editable == "Yes").OrderBy(b => b.ChallanNo);
            return data;
        }

        public DeliveryChallan GetDetailsByChallanNo(string no)
        {
            var data = _DeliveryChallanRepository.Get(n => n.ChallanNo == no);
            return data;
        }

        public DeliveryChallan GetDetailsById(int id)
        {
            var details = _DeliveryChallanRepository.GetById(id);
            return details;
        }

        public IEnumerable<DeliveryChallan> GetAll()
        {
            var data = _DeliveryChallanRepository.GetAll();
            return data;
        }

        public IEnumerable<DeliveryChallan> GetActiveChallanNo(string challanno)
        {
            var data = _DeliveryChallanRepository.GetMany(q => q.Status == "Active" && q.ChallanNo.ToLower().Contains(challanno.ToString().ToLower())).OrderBy(q => q.ChallanNo);
            return data;
        }

        public DeliveryChallan GetDetailsByOrderNo(string orderno)
        {
            var data = _DeliveryChallanRepository.Get(m => m.OrderNo == orderno);
            return data;
        }

        public DeliveryChallan GetDetailsByQuotNo(string quotno)
        {
            var data = _DeliveryChallanRepository.Get(m => m.QuotNo == quotno);
            return data;
        }

        public IEnumerable<DeliveryChallan> GetChallanListByQuotNo(string quotno)
        {
            var list = _DeliveryChallanRepository.GetMany(m => m.QuotNo == quotno);
            return list;
        }

        public IEnumerable<DeliveryChallan> GetChallanListByOrderno(string orderno)
        {
            var list = _DeliveryChallanRepository.GetMany(m => m.OrderNo == orderno);
            return list;
        }

        public IEnumerable<DeliveryChallan> GetDetailsByClientName(string client)
        {
            var details = _DeliveryChallanRepository.GetMany(d => d.ClientName == client && d.Status == "Active");
            return details;
        }

        public IEnumerable<DeliveryChallan> GetDailyChallans()
        {
            var list = _DeliveryChallanRepository.GetMany(d => Convert.ToDateTime(d.ChallanDate).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<DeliveryChallan> GetChallanNoByStatus()
        {
            var list = _DeliveryChallanRepository.GetMany(m => m.Status == "Active");
            return list;
        }

        public IEnumerable<DeliveryChallan> GetAllDeliveryChallans(string term)
        {
            var list = _DeliveryChallanRepository.GetMany(b => b.ChallanNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(b => b.ChallanNo);
            return list;
        }
    }
}
