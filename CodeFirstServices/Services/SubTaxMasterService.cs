using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
   public  class SubTaxMasterService:ISubTaxMasterService
    {
       private readonly IUnitOfWork _unitofwork;
       private readonly ISubTaxMasterRepository _taxrepository;
       public SubTaxMasterService(ISubTaxMasterRepository txsrep, IUnitOfWork iunitofwork)
       {
           this._taxrepository = txsrep;
           this._unitofwork = iunitofwork;
       }

       public void CreateSubTax(SubTaxMaster taxmaster)
       {
           _taxrepository.Add(taxmaster);
           _unitofwork.Commit();

       }

       public void UpdateSubTax(SubTaxMaster tax)
       {
           _taxrepository.Update(tax);
           _unitofwork.Commit();

       }
       public IEnumerable<SubTaxMaster>  getSubTaxMasterDetails(string code)
       {
           var details = _taxrepository.GetMany(sb => sb.Code == code);
           return details;

       }

       public SubTaxMaster getDetailsByCity(string city)
       {
           var details = _taxrepository.Get(ct => ct.City == city);
           return details;

       }

       public SubTaxMaster getDetailsById(int id)
       {
           var details = _taxrepository.Get(sb => sb.Id == id);
           return details;
       }

       public SubTaxMaster getLastInserted()
       {
           var details = _taxrepository.GetAll().LastOrDefault();
           return details;

       }

       public IEnumerable<SubTaxMaster> getDetailsOnSpecifiedId(int fromid, int toid)
       {
           var details = _taxrepository.GetMany(db => db.Id >= fromid && db.Id <= toid);
           return details;
       }
	   
	   public SubTaxMaster GetTaxesByCity(string city,string taxtype)
       {
           var list = _taxrepository.Get(ct => ct.City == city && ct.TaxType == taxtype);
           return list;
       }

       public IEnumerable<SubTaxMaster> getAllTaxesByStates(string vat, string sat, string ot)
       {
           var list = _taxrepository.GetMany(tax => tax.Code == vat && tax.Code == sat && tax.Code == ot);
           return list;
       }
    }
}
