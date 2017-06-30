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
    public class InwardInterGodownService : IInwardInterGodownService
    {
        private readonly IInwardInterGodownRepository _InwardInterGodownRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InwardInterGodownService(IInwardInterGodownRepository InwardInterGodownRepository, IUnitOfWork unitOfWork)
        {
            this._InwardInterGodownRepository = InwardInterGodownRepository;
            this._unitOfWork = unitOfWork;
        }

        public InwardInterGodown GetLastInward()
        {
            var details = _InwardInterGodownRepository.GetAll().LastOrDefault();
            return details;
        }


        public void Create(InwardInterGodown inward)
        {
            _InwardInterGodownRepository.Add(inward);
            _unitOfWork.Commit();
        }


        public InwardInterGodown GetDetailsById(int Id)
        {
            var details = _InwardInterGodownRepository.GetById(Id);
            return details;
        }

        public IEnumerable<InwardInterGodown> GetInwardNo(string term, string GdCode)
        {
            var details = _InwardInterGodownRepository.GetMany(i => i.InwardCode.ToLower().Contains(term.ToLower()) && i.ToGodownCode == GdCode).OrderBy(i => i.InwardCode);
            return details;
        }

        public InwardInterGodown GetDetailsByInwardCode(string InwardCode)
        {
            var details = _InwardInterGodownRepository.Get(i => i.InwardCode == InwardCode);
            return details;
        }


        public IEnumerable<InwardInterGodown> GetDetailsByDate(DateTime FromDate, DateTime ToDate)
        {
            var details = _InwardInterGodownRepository.GetMany(m => Convert.ToDateTime(m.Date) >= FromDate.Date && Convert.ToDateTime(m.Date) <= ToDate.Date);
            return details;
        }

        public IEnumerable<InwardInterGodown> GetDailyInwardInterGodown()
        {
            var list = _InwardInterGodownRepository.GetMany(i => Convert.ToDateTime(i.Date).Date == DateTime.Now.Date);
            return list;
        }


        public InwardInterGodown GetLastInwardByFinYear(string Year,string GdCode)
        {
            var details = _InwardInterGodownRepository.GetMany(p => p.InwardCode.Contains(Year) && p.ToGodownCode == GdCode).OrderBy(p => p.InwardCode).LastOrDefault();
            return details;
        }
    }
}
