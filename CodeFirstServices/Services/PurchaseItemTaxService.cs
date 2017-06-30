using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class PurchaseItemTaxService : IPurchaseItemTaxService
    {
        private readonly IPurchaseItemTaxRepository _purchasetaxitemRepository;
        private readonly IUnitOfWork _unitofwork;

        public PurchaseItemTaxService(IPurchaseItemTaxRepository purchasetaxitemRepository, IUnitOfWork unitofwork)
        {
            this._purchasetaxitemRepository = purchasetaxitemRepository;
            this._unitofwork = unitofwork;
        }

        public void Create(PurchaseItemTaxMaster tax)
        {
            _purchasetaxitemRepository.Add(tax);
            _unitofwork.Commit();
        }

        public IEnumerable<PurchaseItemTaxMaster> GetAll()
        {
            var list = _purchasetaxitemRepository.GetAll();
            return list;
        }

        public IEnumerable<PurchaseItemTaxMaster> GetDetailsByDate(DateTime fromdate, DateTime todate, string taxtype)
        {
            if (taxtype != "All")
            {
                var list = _purchasetaxitemRepository.GetMany(l => Convert.ToDateTime(l.FromDate).Date <= fromdate.Date && Convert.ToDateTime(l.ToDate).Date >= todate.Date && l.TaxType == taxtype);
                return list;
            }
            else
            {
                var list = _purchasetaxitemRepository.GetMany(l => Convert.ToDateTime(l.FromDate).Date <= fromdate.Date && Convert.ToDateTime(l.ToDate).Date >= todate.Date);
                return list;
            }
        }

        public PurchaseItemTaxMaster GetLastRow()
        {
            var row = _purchasetaxitemRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<PurchaseItemTaxMaster> GetInsertedRows(int lastrowbefore, int lastrowafter)
        {
            var data = _purchasetaxitemRepository.GetMany(m => m.TaxItemId >= lastrowbefore && m.TaxItemId <= lastrowafter);
            return data;
        }


        public PurchaseItemTaxMaster GetLatestTax(string type, string subcat)
        {
            var data = _purchasetaxitemRepository.GetMany(m => DateTime.Now.Date >= Convert.ToDateTime(m.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(m.ToDate).Date && m.TaxType == type && m.SubCategory == subcat).LastOrDefault();
            return data;
        }

        public PurchaseItemTaxMaster GetTaxBySubCatAndVAT(string subcat)
        {
            var data = _purchasetaxitemRepository.GetMany(m => DateTime.Now.Date >= Convert.ToDateTime(m.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(m.ToDate).Date && m.SubCategory == subcat && m.TaxType == "VAT").LastOrDefault();
            return data;
        }

        public PurchaseItemTaxMaster GetLatestTaxByState(string type, string subcat, string state)
        {
            var data = _purchasetaxitemRepository.GetMany(m => DateTime.Now.Date >= Convert.ToDateTime(m.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(m.ToDate).Date && m.TaxType == type && m.SubCategory == subcat && m.State == state).LastOrDefault();
            return data;
        }
    }
}
