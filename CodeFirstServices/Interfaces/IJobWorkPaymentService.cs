using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IJobWorkPaymentService
    {
        void Create(JobWorkPayment data);
        void Update(JobWorkPayment data);
        JobWorkPayment GetLastPaymentByFinYr(string year);
        IEnumerable<JobWorkPayment> GetRowsByOutwardNo(string outwardno);
        IEnumerable<JobWorkPayment> GetBillsByHandoverStatus();
        JobWorkPayment GetRowsByOutwardToTailorAndPaymentNo(string outtotailorno, string paymentno);
        IEnumerable<JobWorkPayment> GetHandoverJobWorkPayment();
    }
}
