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
    public class OutwardInterGodownService : IOutwardInterGodownService
    {
        private readonly IOutwardInterGodownRepository _OutwardInterGodownRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardInterGodownService(IOutwardInterGodownRepository OutwardInterGodownRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardInterGodownRepository = OutwardInterGodownRepository;
            this._unitOfWork = unitOfWork;
        }

        public OutwardInterGodown GetLastRow()
        {
            var data = _OutwardInterGodownRepository.GetAll().LastOrDefault();
            return data;
        }

        public void Create(OutwardInterGodown outward)
        {
            _OutwardInterGodownRepository.Add(outward);
            _unitOfWork.Commit();
        }


        public OutwardInterGodown GetDetailsByOutwardId(int id)
        {
            var details = _OutwardInterGodownRepository.Get(m => m.OutwardId == id);
            return details;
        }

        public IEnumerable<OutwardInterGodown> GetReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _OutwardInterGodownRepository.GetMany(cl => Convert.ToDateTime(cl.OutwardDate).Date >= fromdate.Date && Convert.ToDateTime(cl.OutwardDate).Date <= todate);
            return value;
        }

        public IEnumerable<OutwardInterGodown> GetOutwardNos(string term, string GodownCode)
        {
            var details = _OutwardInterGodownRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.ToGodownCode == GodownCode && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public OutwardInterGodown GetDetailsByOutwardCode(string OutwardCode)
        {
            var details = _OutwardInterGodownRepository.Get(o => o.OutwardCode == OutwardCode);
            return details;
        }

        public void Update(OutwardInterGodown outward)
        {
            _OutwardInterGodownRepository.Update(outward);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardInterGodown> GetOutwardNoForPrint(string term, string GodownCode)
        {
            var details = _OutwardInterGodownRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.FromGodownCode == GodownCode).OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardInterGodown> GetDailyOutwardInterGodown()
        {
            var list = _OutwardInterGodownRepository.GetMany(o => Convert.ToDateTime(o.OutwardDate).Date == DateTime.Now.Date);
            return list;
        }

        public OutwardInterGodown GetLastOutwardByFinYr(string year,string GdCode)
        {
            var data = _OutwardInterGodownRepository.GetMany(o => o.OutwardCode.Contains(year) && o.FromGodownCode == GdCode).OrderBy(o => o.OutwardCode).LastOrDefault();
            return data;
        }

        public void Delete(OutwardInterGodown outward)
        {
            _OutwardInterGodownRepository.Delete(outward);
            _unitOfWork.Commit();
        }
    }
}
