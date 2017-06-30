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
    public class InwardFromGodownService : IInwardFromGodownService
    {
        private readonly IInwardFromGodownRepository _inwardfromgodownrepository;
        private readonly IUnitOfWork _iunitofwork;

        public InwardFromGodownService(IInwardFromGodownRepository inwardfromgodownrepository, IUnitOfWork iunitofwork)
        {
            this._inwardfromgodownrepository = inwardfromgodownrepository;
            this._iunitofwork = iunitofwork;
        }

        public void Create(InwardFromGodown InwardFromGodown)
        {
            _inwardfromgodownrepository.Add(InwardFromGodown);
            _iunitofwork.Commit();
        }

        public InwardFromGodown GetLastRow()
        {
            var data = _inwardfromgodownrepository.GetAll().LastOrDefault();
            return data;
        }

        public IEnumerable<InwardFromGodown> GetAll()
        {
            var list = _inwardfromgodownrepository.GetAll();
            return list;
        }

        public InwardFromGodown GetDetailsByCode(string code)
        {
            var data = _inwardfromgodownrepository.Get(d => d.InwardCode == code);
            return data;
        }

        public InwardFromGodown GetDetailsById(int id)
        {
            var data = _inwardfromgodownrepository.GetById(id);
            return data;
        }

        public IEnumerable<InwardFromGodown> GetReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _inwardfromgodownrepository.GetMany(cl => Convert.ToDateTime(cl.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(cl.ModifiedOn).Date <= todate);
            return value;
        }

        public IEnumerable<InwardFromGodown> GetReportByGodownNameAndDate(string Godown, DateTime fromdate, DateTime todate)
        {
            var data = _inwardfromgodownrepository.GetMany(d => d.GodownName == Godown && Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date);
            return data;
        }


        public IEnumerable<InwardFromGodown> GetInwardNo(string term, string shopcode)
        {
            var details = _inwardfromgodownrepository.GetMany(i => i.InwardCode.ToLower().Contains(term.ToLower()) && i.ShopCode == shopcode).OrderBy(i => i.InwardCode);
            return details;
        }

        public IEnumerable<InwardFromGodown> GetDailyInwardsFromGodown()
        {
            var list = _inwardfromgodownrepository.GetMany(i => Convert.ToDateTime(i.Date).Date == DateTime.Now.Date);
            return list;
        }


        public InwardFromGodown GetLastInwardByFinYear(string Year,string ShopCode)
        {
            var details = _inwardfromgodownrepository.GetMany(p => p.InwardCode.Contains(Year) && p.ShopCode == ShopCode).OrderBy(p => p.InwardCode).LastOrDefault();
            return details;
        }
    }
}
