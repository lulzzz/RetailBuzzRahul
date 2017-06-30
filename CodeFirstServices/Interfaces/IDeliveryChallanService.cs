using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IDeliveryChallanService
    {
        void Create(DeliveryChallan DeliveryChallan);
        void Update(DeliveryChallan DeliveryChallan);
        IEnumerable<DeliveryChallan> GetActiveChallanNo(string challanno);
        DeliveryChallan GetLastChallanByFinYr(string year, string shopcode);
        IEnumerable<DeliveryChallan> GetChallanNos(string billno);
        DeliveryChallan GetDetailsByChallanNo(string no);
        DeliveryChallan GetDetailsById(int id);
        IEnumerable<DeliveryChallan> GetAll();
        IEnumerable<DeliveryChallan> GetAllDeliveryChallans(string term);
        DeliveryChallan GetDetailsByOrderNo(string orderno);
        DeliveryChallan GetDetailsByQuotNo(string quotno);
        IEnumerable<DeliveryChallan> GetChallanListByQuotNo(string quotno);
        IEnumerable<DeliveryChallan> GetChallanListByOrderno(string orderno);
        IEnumerable<DeliveryChallan> GetDetailsByClientName(string client);
        IEnumerable<DeliveryChallan> GetDailyChallans();
        IEnumerable<DeliveryChallan> GetChallanNoByStatus();
    }
}
