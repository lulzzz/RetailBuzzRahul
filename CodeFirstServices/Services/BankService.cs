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
    public class BankService: IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BankService(IBankRepository bankRepository, IUnitOfWork unitOfWork)
        {
            this._bankRepository = bankRepository;
            this._unitOfWork = unitOfWork;
        }


        public BankMaster getById(int id)
        {
            var data=_bankRepository.Get(List=>List.bankId==id);
            return data;
        }

        public IEnumerable<BankMaster> getAllBanks()
        {
            var bank = _bankRepository.GetAll();
            return bank;
        }

        public BankMaster getLastInsertedBank()
        {
            var bank = _bankRepository.GetAll().LastOrDefault();
            return bank;
        }

        public BankMaster getBankByifsc(string ifsc)
        {
            var bnkname = _bankRepository.Get(bnk => bnk.IFSC == ifsc);
            return bnkname;
        }


        public IEnumerable<BankMaster> GetIfscByBank(string id)
        {
            var ifscList = _bankRepository.GetMany(bnk => bnk.bankName == id);
            return ifscList;
        }

        public IEnumerable<BankMaster> GetIfscByBankWithNotSelectedValue(string bankName, string bankId)
        {
            var ifscList = _bankRepository.GetMany(bnk => bnk.bankName == bankName && bnk.IFSC != bankId);
            return ifscList;
        }

        public void CreateBank(BankMaster Bank)
        {
            _bankRepository.Add(Bank);
            _unitOfWork.Commit();
        }

        public void UpdateBank(BankMaster Bank)
        {
            _bankRepository.Update(Bank);
            _unitOfWork.Commit();
        }

        public string GetAddressByIFSC(string IFSCNo)
        {
            var BankAddress = _bankRepository.Get(m => m.IFSC == IFSCNo);
            return BankAddress.address;
        }
        public int GetIdByName(string bankname)
        {
            var idbank = _bankRepository.Get(m => m.bankName == bankname);
            return idbank.bankId;
        }

        public IEnumerable<BankMaster> GetBranchByBankName(string bankname)
        {
            var branchlist = _bankRepository.GetMany(bank => bank.bankName == bankname);
            return branchlist;
        }

        public string GetIfscByBranch(string branch)
        {
            var ifscnolist = _bankRepository.Get(bank => bank.Branch == branch);
            return ifscnolist.IFSC;
        }

        public IEnumerable<BankMaster> GetBankNameList(string name)
        {
            var list = _bankRepository.GetMany(l1 => l1.bankName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(n => n.bankName);
            return list;
        }

        public IEnumerable<BankMaster> GetBranchFromBankName(string name)
        {
            var list = _bankRepository.GetMany(l => l.bankName == name && l.status == "Active");
            return list;
        }

        public BankMaster GetBankDetailsById(int id)
        {
            var details = _bankRepository.GetById(id);
            return details;
        }

        public IEnumerable<BankMaster> GetIFSCList(string ifsc)
        {
            var list = _bankRepository.GetMany(l => l.IFSC == ifsc);
            return list;
        }

        public IEnumerable<BankMaster> GetEmailList(string Email)
        {
            var list = _bankRepository.GetMany(l => l.email == Email);
            return list;
        }

        public BankMaster GetMICRByIFSC(string IFSC)
        {
            var details = _bankRepository.Get(bnk => bnk.IFSC == IFSC);
            return details;
        }
    }
}
