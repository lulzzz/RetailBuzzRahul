using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISupplierBankDetailService
    {
        void CreateBankDetails(SupplierBankDetail supplierbankdetail);
        void UpdateBankDetails(SupplierBankDetail supplierbankdetail);
        IEnumerable<SupplierBankDetail> GetDetailsFromBank(string suppliercode);
        void ExecuteProcedure(string suppliercode);
        SupplierBankDetail GetBankDetailsOfSupplier(int selectedbankname);
        IEnumerable<SupplierBankDetail> GetAllSupplierBanks();
    }
}
