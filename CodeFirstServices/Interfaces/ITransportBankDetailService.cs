using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ITransportBankDetailService
    {
        void CreateBankDetails(TransportBankDetail TransportBankDetail);
        void UpdateBankDetails(TransportBankDetail TransportBankDetail);
        IEnumerable<TransportBankDetail> GetDetailsFromBank(string transportcode);
        void ExecuteProcedure(string transportcode);
        TransportBankDetail GetBankDetailsOfTransport(int selectedbankname);
        IEnumerable<TransportBankDetail> GetAllTransportBanks();
    }
}
