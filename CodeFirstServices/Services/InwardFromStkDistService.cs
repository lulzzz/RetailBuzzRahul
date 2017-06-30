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
    public class InwardFromStkDistService : IInwardFromStkDistService
    {
        private readonly IInwardFromStkDistRepository _InwardStkDistRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InwardFromStkDistService(IInwardFromStkDistRepository InwardStkDistRepository, IUnitOfWork unitOfWork)
        {
            this._InwardStkDistRepository = InwardStkDistRepository;
            this._unitOfWork = unitOfWork;
        }

        public InwardFromStockDistribution GetLastRow()
        {
            var data = _InwardStkDistRepository.GetAll().LastOrDefault();
            return data;
        }

        public void Create(InwardFromStockDistribution item)
        {
            _InwardStkDistRepository.Add(item);
            _unitOfWork.Commit();
        }

        public IEnumerable<InwardFromStockDistribution> GetReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _InwardStkDistRepository.GetMany(cl => Convert.ToDateTime(cl.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(cl.ModifiedOn).Date <= todate);
            return value;
        }

        public InwardFromStockDistribution GetDetailsById(int id)
        {
            var det = _InwardStkDistRepository.GetById(id);
            return det;
        }


        public IEnumerable<InwardFromStockDistribution> GetInwardNo(string term, string code)
        {
            var details = _InwardStkDistRepository.GetMany(i => i.InwardNo.ToLower().Contains(term.ToLower()) && i.Code == code).OrderBy(i => i.InwardNo);
            return details;
        }


        public InwardFromStockDistribution GetDetailsByInwardNo(string InwardNo)
        {
            var details = _InwardStkDistRepository.Get(i => i.InwardNo == InwardNo);
            return details;
        }

        public InwardFromStockDistribution GetLastInwardByFinYearForShop(string Year, string ShopCode)
        {
            var details = _InwardStkDistRepository.GetMany(i => i.InwardNo.Contains(Year) && i.Code == ShopCode).OrderBy(o => o.InwardNo).LastOrDefault();
            return details;
        }


        public InwardFromStockDistribution GetLastInwardByFinYearForGodown(string Year, string GdCode)
        {
            var details = _InwardStkDistRepository.GetMany(i => i.InwardNo.Contains(Year) && i.Code == GdCode).OrderBy(o => o.InwardNo).LastOrDefault();
            return details;
        }
    }
}
