using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;

namespace CodeFirstServices.Services
{
    public class ClientBankDetailService : IClientBankDetailService
    {
        private readonly IClientBankDetailRepository _ClientBankDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ClientBankDetailService(IClientBankDetailRepository ClientBankDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._ClientBankDetailRepository = ClientBankDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void CreateBankDetails(ClientBankDetail clientbankdetail)
        {
            _ClientBankDetailRepository.Add(clientbankdetail);
            _UnitOfWork.Commit();
        }

        public void UpdateBankDetails(ClientBankDetail clientbankdetail)
        {
            _ClientBankDetailRepository.Update(clientbankdetail);
            _UnitOfWork.Commit();
        }

        public IEnumerable<ClientBankDetail> GetDetailsFromBank(string clientcode)
        {
            var clientCode = _ClientBankDetailRepository.GetMany(code => code.ClientCode == clientcode);
            return clientCode;
        }

        public void ExecuteProcedure(string clientcode)
        {
            var banklist = _ClientBankDetailRepository.GetMany(code => code.ClientCode == clientcode);
            foreach (var item in banklist)
            {
                _ClientBankDetailRepository.Delete(item);
                _UnitOfWork.Commit();
            }
        }

        public ClientBankDetail GetBankDetailsOfClient(int selectedbankname)
        {
            var banklist = _ClientBankDetailRepository.Get(bank => bank.BankId == selectedbankname);
            return banklist;
        }

        public IEnumerable<ClientBankDetail> GetAllClientBanks()
        {
            var list = _ClientBankDetailRepository.GetAll();
            return list;
        }

        public ClientBankDetail GetBankDetailsByClientCode(string Code)
        {
            var details = _ClientBankDetailRepository.Get(cl => cl.ClientCode == Code);
            return details;
        }
    }
}
