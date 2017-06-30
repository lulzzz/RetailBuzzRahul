using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class InwardFromShopToGodownService : IInwardFromShopToGodownService
    {
        private readonly IInwardFromShopToGodownRepository _InwardFromShopToGodownRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InwardFromShopToGodownService(IInwardFromShopToGodownRepository InwardFromShopToGodownRepository, IUnitOfWork unitOfWork)
        {
            this._InwardFromShopToGodownRepository = InwardFromShopToGodownRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(InwardFromShopToGodown Inward)
        {
            _InwardFromShopToGodownRepository.Add(Inward);
            _unitOfWork.Commit();
        }

        public void Update(InwardFromShopToGodown Inward)
        {
            _InwardFromShopToGodownRepository.Update(Inward);
            _unitOfWork.Commit();
        }
        public IEnumerable<InwardFromShopToGodown> GetDetailsByDate(DateTime FromDate, DateTime ToDate)
        {
            var details = _InwardFromShopToGodownRepository.GetMany(i => Convert.ToDateTime(i.Date) >= FromDate.Date && Convert.ToDateTime(i.Date) <= ToDate.Date);
            return details;
        }

        public IEnumerable<InwardFromShopToGodown> GetDetailsByShopAndDate(string Shop, DateTime FromDate, DateTime ToDate)
        {
            var details = _InwardFromShopToGodownRepository.GetMany(i => i.ShopName == Shop && Convert.ToDateTime(i.Date) >= FromDate.Date && Convert.ToDateTime(i.Date) <= ToDate.Date);
            return details;
        }

        public InwardFromShopToGodown GetInwardDetailById(int id)
        {
            var details = _InwardFromShopToGodownRepository.GetById(id);
            return details;
        }

        public InwardFromShopToGodown GetLastInward()
        {
            var details = _InwardFromShopToGodownRepository.GetAll().LastOrDefault();
            return details;
        }

        public IEnumerable<InwardFromShopToGodown> GetInwardNo(string term, string godowncode)
        {
            var details = _InwardFromShopToGodownRepository.GetMany(i => i.InwardCode.ToLower().Contains(term.ToLower()) && i.GodownCode == godowncode).OrderBy(i => i.InwardCode);
            return details;
        }


        public InwardFromShopToGodown GetDetailsByInwardCode(string InwardCode)
        {
            var details = _InwardFromShopToGodownRepository.Get(i => i.InwardCode == InwardCode);
            return details;
        }

        public IEnumerable<InwardFromShopToGodown> GetDebitNoteNos(string term)
        {
            var list = _InwardFromShopToGodownRepository.GetMany(i => i.DebitNoteNo.ToLower().StartsWith(term.ToLower()) && i.Status == "Active" && i.StockReturn == "Yes").OrderBy(i => i.DebitNoteNo);
            return list;
        }

        public InwardFromShopToGodown GetDataByDebitNoteNo(string debitnoteno)
        {
            var data = _InwardFromShopToGodownRepository.Get(i => i.DebitNoteNo == debitnoteno);
            return data;
        }
        
        public IEnumerable<InwardFromShopToGodown> GetDailyInwardsShopToGodown()
        {
            var list = _InwardFromShopToGodownRepository.GetMany(i => Convert.ToDateTime(i.Date).Date == DateTime.Now.Date);
            return list;
        }


        public InwardFromShopToGodown GetLastInwardByFinYear(string Year,string GdCode)
        {
            var details = _InwardFromShopToGodownRepository.GetMany(i => i.InwardCode.Contains(Year) && i.GodownCode == GdCode).OrderBy(i => i.InwardCode).LastOrDefault();
            return details;
        }
    }
}
