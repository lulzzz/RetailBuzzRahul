using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IClientMasterService
    {
        ClientMaster GetLastInsertedClient();
        ClientMaster getById(int id);
        void CreateClient(ClientMaster Client);
        void UpdateClient(ClientMaster Client);
        IEnumerable<ClientMaster> GetEmail(string mail);
        IEnumerable<ClientMaster> GetNameByCard(string membershipcard);
        ClientMaster getClientById(int id);
        IEnumerable<ClientMaster> GetClientNames(string name);
        IEnumerable<ClientMaster> ValidateName(string clientname);
        IEnumerable<ClientMaster> GetAllClients();
        ClientMaster getClientByName(string name);
        IEnumerable<ClientMaster> GetActiveAndMaharashtraClients(string name);
        IEnumerable<ClientMaster> GetActiveClients();
    }
}
