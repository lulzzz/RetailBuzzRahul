using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IClientBankDetailService
    {
        void CreateBankDetails(ClientBankDetail clientbankdetail);
        void UpdateBankDetails(ClientBankDetail clientbankdetail);
        IEnumerable<ClientBankDetail> GetDetailsFromBank(string clientcode);
        void ExecuteProcedure(string  clientcode);
        ClientBankDetail GetBankDetailsOfClient(int selectedbankname);
        IEnumerable<ClientBankDetail> GetAllClientBanks();
        ClientBankDetail GetBankDetailsByClientCode(string Code);
    }
}
