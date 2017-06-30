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
    public class OutwardToTailorService : IOutwardToTailorService
    {
        private readonly IOutwardToTailorRepository _OutwardToTailorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardToTailorService(IOutwardToTailorRepository OutwardToTailorRepository, IUnitOfWork unitOfWork)
        {
            this._OutwardToTailorRepository = OutwardToTailorRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardToTailor outward)
        {
            _OutwardToTailorRepository.Add(outward);
            _unitOfWork.Commit();
        }

        public void Update(OutwardToTailor outward)
        {
            _OutwardToTailorRepository.Update(outward);
            _unitOfWork.Commit();
        }

        public OutwardToTailor GetLastRow()
        {
            var data = _OutwardToTailorRepository.GetAll().LastOrDefault();
            return data;
        }

        public OutwardToTailor GetDetailsById(int id)
        {
            var data = _OutwardToTailorRepository.GetById(id);
            return data;
        }

        public OutwardToTailor GetLastRowrByFinYr(string year)
        {
            var data = _OutwardToTailorRepository.GetMany(p => p.OutwardCode.Contains(year)).OrderBy(p => p.OutwardCode).LastOrDefault();
            return data;
        }

        public IEnumerable<OutwardToTailor> GetOutwardNo(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToTailor> GetOutwardNoByStatusAndInwardStatus(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.Status == "Active" && m.InwardStatus == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public OutwardToTailor GetDetailsByCode(string code)
        {
            var data = _OutwardToTailorRepository.Get(d => d.OutwardCode == code);
            return data;
        }

        public IEnumerable<OutwardToTailor> GetOutwardNoByInwardStatus(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.InwardStatus == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToTailor> GetPendingOutwardToTailors()
        {
            var data = _OutwardToTailorRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<OutwardToTailor> GetRowsByStatusAndInwardStatus(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.Status == "Active" && m.InwardStatus == "InActive").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToTailor> GetRowsByTailotName(string name)
        {
            var data = _OutwardToTailorRepository.GetMany(d => d.TailorName == name && d.TailorInwardStatus == "Active");
            return data;
        }

        public IEnumerable<OutwardToTailor> GetOutwardNoByInActiveInwardStatus(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.InwardStatus == "InActive").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToTailor> GetOutwardNoByInwardAndsTailorInwardStatus(string term)
        {
            var details = _OutwardToTailorRepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.InwardStatus == "Active" && m.TailorInwardStatus == "InActive").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToTailor> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate)
        {
            var data = _OutwardToTailorRepository.GetMany(d => d.TailorName == Tailor && d.Date.Date >= fromdate.Date && d.Date.Date <= todate.Date);
            return data;
        }

    }
}
