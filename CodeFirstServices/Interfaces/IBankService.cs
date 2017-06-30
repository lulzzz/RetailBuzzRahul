using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IBankService
    {
        BankMaster getById(int id);
        IEnumerable<BankMaster> getAllBanks();
        BankMaster getLastInsertedBank();
        IEnumerable<BankMaster> GetIfscByBank(string id);
        BankMaster getBankByifsc(string ifsc);
        IEnumerable<BankMaster> GetIfscByBankWithNotSelectedValue(string bankName, string bankId);
        void CreateBank(BankMaster Bank);
        void UpdateBank(BankMaster Bank);
        string GetAddressByIFSC(string Address);
        int GetIdByName(string bankname);
        IEnumerable<BankMaster> GetBranchByBankName(string bankname);
        string GetIfscByBranch(string branch);
        IEnumerable<BankMaster> GetBankNameList(string name);
        IEnumerable<BankMaster> GetBranchFromBankName(string name);
        BankMaster GetBankDetailsById(int id);
        IEnumerable<BankMaster> GetIFSCList(string ifsc);
        IEnumerable<BankMaster> GetEmailList(string Email);
        BankMaster GetMICRByIFSC(string IFSC);
        }
}
