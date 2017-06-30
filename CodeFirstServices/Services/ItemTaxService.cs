using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class ItemTaxService : IItemTaxService
    {
        private readonly IItemTaxRepository _taxitemRepository;
        private readonly IUnitOfWork _unitofwork;

        public ItemTaxService(IItemTaxRepository taxitemRepository, IUnitOfWork unitofwork)
        {
            this._taxitemRepository = taxitemRepository;
            this._unitofwork = unitofwork;
        }

        public void Create(ItemTaxMaster tax)
        {
            _taxitemRepository.Add(tax);
            _unitofwork.Commit();
        }

        public IEnumerable<ItemTaxMaster> GetAll()
        {
            var list = _taxitemRepository.GetAll();
            return list;
        }

        public IEnumerable<ItemTaxMaster> GetDetailsByDate(DateTime fromdate, DateTime todate, string taxtype)
        {
            if (taxtype != "All")
            {
                var list = _taxitemRepository.GetMany(l => Convert.ToDateTime(l.FromDate).Date <= fromdate.Date && Convert.ToDateTime(l.ToDate).Date >= todate.Date && l.TaxType == taxtype);
                return list;
            }
            else 
            {
                var list = _taxitemRepository.GetMany(l => Convert.ToDateTime(l.FromDate).Date <= fromdate.Date && Convert.ToDateTime(l.ToDate).Date >= todate.Date);
                return list;
            }
        }

        public ItemTaxMaster GetLastRow()
        {
            var row = _taxitemRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<ItemTaxMaster> GetInsertedRows(int lastrowbefore,int lastrowafter)
        {
            var data = _taxitemRepository.GetMany(m => m.TaxItemId >= lastrowbefore && m.TaxItemId<=lastrowafter);
            return data;
        }


        public ItemTaxMaster GetLatestTax(string type,string subcat)
        {
            var data = _taxitemRepository.GetMany(m => DateTime.Now.Date >= Convert.ToDateTime(m.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(m.ToDate).Date && m.TaxType == type && m.SubCategory == subcat).LastOrDefault();
            return data;
        }

        public ItemTaxMaster GetTaxBySubCatAndVAT(string subcat)
        {
            var data = _taxitemRepository.GetMany(m => DateTime.Now.Date >= Convert.ToDateTime(m.FromDate).Date && DateTime.Now.Date <= Convert.ToDateTime(m.ToDate).Date && m.SubCategory == subcat && m.TaxType == "VAT").LastOrDefault();
            return data;
        }
       
    }
}
