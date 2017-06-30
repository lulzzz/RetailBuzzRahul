using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
   public  class TaxService:ITaxService
    {
       private readonly IUnitOfWork _unitofwork;
       private readonly ITaxMasterRepository _taxrepository;
       public TaxService(ITaxMasterRepository txsrep, IUnitOfWork iunitofwork)
       {
           this._taxrepository = txsrep;
           this._unitofwork = iunitofwork;
       }

       public void CreateTax(MainTaxMaster taxmaster)
       {
           _taxrepository.Add(taxmaster);
           _unitofwork.Commit();
            
       }

       public MainTaxMaster getLastInsertedCompany()
       {
           var tax = _taxrepository.GetAll().LastOrDefault();
           return tax;
       }

       public MainTaxMaster getAllStateDetails(string state)
       {
           var statelist = _taxrepository.Get(st => st.State == state);
           return statelist;

       }


       public MainTaxMaster getLastInsertedTax()
       {
           var tax = _taxrepository.GetAll().LastOrDefault();
           return tax;
       }

       public MainTaxMaster GetById(int id)
       {
           var tax = _taxrepository.GetById(id);
           return tax;
       }
    }
}

