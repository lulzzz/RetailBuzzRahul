using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface ICommissionProductService
    {
        void Create(CommissionProduct CommProduct);
        IEnumerable<CommissionProduct> GetDetailsByCommCode(string Code);
        IEnumerable<CommissionProduct> GetAllEmployees();
        IEnumerable<CommissionProduct> GetDetailsByEmployeeName(string Name);
        CommissionProduct GetSingleRowByCommCode(string Code);
        void Update(CommissionProduct comm);
        void Delete(CommissionProduct Comm);
    }
}
