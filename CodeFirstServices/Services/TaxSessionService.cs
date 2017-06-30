using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class TaxSessionService : ITaxSessionService
    {
        private readonly ITaxSessionRepository _taxsessionRepository;
        private readonly IUnitOfWork _unitofwork;

        public TaxSessionService(ITaxSessionRepository taxsessionRepository, IUnitOfWork unitofwork)
        {
            this._taxsessionRepository = taxsessionRepository;
            this._unitofwork = unitofwork;
        }

        public void Create(TaxSessionMaster tax)
        {
            _taxsessionRepository.Add(tax);
            _unitofwork.Commit();
        }

        public TaxSessionMaster GetLastRow()
        {
            var row = _taxsessionRepository.GetAll().LastOrDefault();
            return row;
        }

        public TaxSessionMaster GetDetailsById(int id)
        {
            var data = _taxsessionRepository.GetById(id);
            return data;
        }

        public TaxSessionMaster GetDetailsByTaxCode(string code)
        {
            var data = _taxsessionRepository.Get(d => d.Taxcode == code);
            return data;
        }

        public IEnumerable<TaxSessionMaster> GetDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var list = _taxsessionRepository.GetMany(l => Convert.ToDateTime(l.FromDate).Date >= fromdate.Date && Convert.ToDateTime(l.ToDate).Date <= todate.Date);
            return list;
        }

    }
}
