 using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
  public  class CompanyBankDetailService:ICompanyBankDetailsService
    {

        private readonly IComapnyBankDetailsRepository _compbankdetailsrep;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyBankDetailService(IComapnyBankDetailsRepository companybnkRepository, IUnitOfWork unitOfWork)
        {
            this._compbankdetailsrep = companybnkRepository;
            this._unitOfWork = unitOfWork;
        }

      public void CreateCompanyBankDetails(companybankdetail Combankdetails)
        {
            _compbankdetailsrep.Add(Combankdetails);
            _unitOfWork.Commit();
        }
      public void UpdateBank(companybankdetail compbankdetails)
      {
          _compbankdetailsrep.Update(compbankdetails);
          _unitOfWork.Commit();
      }
      public IEnumerable<companybankdetail> getBankDetailsByCompanyCode(string companycode)
      {
          var bnkdetails = _compbankdetailsrep.GetMany(bnk => bnk.CompanyCode == companycode);
          return bnkdetails;

      }

      public IEnumerable<companybankdetail> getAllBankDetails()
      {
          var bnkdetails = _compbankdetailsrep.GetAll();
          return bnkdetails;

      }
      public companybankdetail getlastInsertedbnk()
      {
          var bnk = _compbankdetailsrep.GetAll().LastOrDefault();
          return bnk;
      }

     public void ExecuteProcedure(string code)
      {
          var banklist = _compbankdetailsrep.GetMany(bnk => bnk.CompanyCode == code);
          foreach (var item in banklist)
          {
              _compbankdetailsrep.Delete(item);
              _unitOfWork.Commit();

          }

      }

     public IEnumerable<companybankdetail> BankInfo(string code)
     {
         var bnklist = _compbankdetailsrep.GetMany(bnk => bnk.CompanyCode == code);
         return bnklist;

     }

     public companybankdetail getBankDetailsById(int id)
     {
         var bnkdetails = _compbankdetailsrep.Get(bnk => bnk.BankId==id);
         return bnkdetails;

     }


    }
}
