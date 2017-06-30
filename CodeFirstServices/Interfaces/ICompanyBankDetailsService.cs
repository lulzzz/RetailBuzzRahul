using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface ICompanyBankDetailsService
    {
        void CreateCompanyBankDetails(companybankdetail Combankdetails);
        IEnumerable<companybankdetail> getAllBankDetails();
        companybankdetail getlastInsertedbnk();
        IEnumerable<companybankdetail> getBankDetailsByCompanyCode(string companycode);
        void ExecuteProcedure(string code);
        IEnumerable<companybankdetail> BankInfo(string code);
        companybankdetail getBankDetailsById(int id);
        void UpdateBank(companybankdetail compbankdetails);
    }
}
