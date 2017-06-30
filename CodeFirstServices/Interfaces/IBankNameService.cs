using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IBankNameService
    {
        void CreateBankName(BankName Bank);
        IEnumerable<BankName> getAllBankNames();
        string GetBankyNamebyId(int id);
        IEnumerable<BankName> GetBankNames(string bankname);
    }
}
