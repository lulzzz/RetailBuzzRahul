using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISuppliersMasterService
    {
        IEnumerable<SupplierMaster> getAllSuppliers();
        SupplierMaster getLastInsertedSupplier();
        SupplierMaster getById(int id);
        void CreateSupplier(SupplierMaster Supplier);
        void UpdateSupplier(SupplierMaster Supplier);
        void DeleteSupplier(SupplierMaster Supplier);
        IEnumerable<SupplierMaster> GetEmail(string mail);
        IEnumerable<SupplierMaster> GetSupplierNames(string name);
        IEnumerable<SupplierMaster> ValidateName(string suppliername);
        SupplierMaster GetByName(string name);
        IEnumerable<SupplierMaster> LoadSupplierNameBySupplierType(string name);
        IEnumerable<SupplierMaster> GetActiveSuppliers();
        IEnumerable<SupplierMaster> GetActiveSupplersByName(string name);
    }
}
