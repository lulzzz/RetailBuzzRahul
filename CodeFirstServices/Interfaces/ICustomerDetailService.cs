using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICustomerDetailService
    {
        void CreateInvoice(CustomerDetail customerdetail);
        void UpdateInvoice(CustomerDetail customerdetail);
        CustomerDetail GetById(int id);
        IEnumerable<CustomerDetail> GetAll();
        CustomerDetail GetLastInvoice();
        IEnumerable<CustomerDetail> GetCustomerName(string name);
        CustomerDetail GetDetailsByName(string name);
    }
}
