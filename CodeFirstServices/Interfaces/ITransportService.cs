using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;


namespace CodeFirstServices.Interfaces
{
    public interface ITransportService
    {
        IEnumerable<TransportMaster> GetTransportByMode(string transport);
        TransportMaster getLastInsertedTransport();
        TransportMaster getById(int id);
        void CreateTransport(TransportMaster Transport);
        void UpdateTransport(TransportMaster Transport);
        IEnumerable<TransportMaster> GetEmail(string mail);
        IEnumerable<TransportMaster> GetTranportNames(string name);
        IEnumerable<TransportMaster> ValidateName(string transportname);
        TransportMaster GetByName(string name);
       // IEnumerable<TransportMaster> getAllTransport();
        IEnumerable<TransportMaster> GetAllTransport();
    }
}
