using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class ClientMasterService : IClientMasterService
    {
        private readonly IClientMasterRepository _clientMasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ClientMasterService(IClientMasterRepository clientMasterRepository, IUnitOfWork unitOfWork)
        {
            this._clientMasterRepository = clientMasterRepository;
            this._unitOfWork = unitOfWork;
        }

        public ClientMaster GetLastInsertedClient()
        {
            var client = _clientMasterRepository.GetAll().LastOrDefault();
            return client;

        }
        public ClientMaster getClientByName(string name)
        {
            var companyName = _clientMasterRepository.Get(cmp => cmp.ClientName == name);
            return companyName;
        }


        public ClientMaster getById(int id)
        {
            var client = _clientMasterRepository.GetById(id);
            return client;
        }

        public void CreateClient(ClientMaster Client)
        {
            _clientMasterRepository.Add(Client);
            _unitOfWork.Commit();
        }

        public void UpdateClient(ClientMaster Client)
        {
            _clientMasterRepository.Update(Client);
            _unitOfWork.Commit();
        }

        public IEnumerable<ClientMaster> GetEmail(string mail)
        {
            var EmailList = _clientMasterRepository.GetMany(cl => cl.Email == mail);
            return EmailList;
        }

        public IEnumerable<ClientMaster> GetNameByCard(string membershipcard)
        {
            var clientList = _clientMasterRepository.GetMany(cl => cl.TypeOfMembershipCard == membershipcard && cl.Status == "Active");
            return clientList;
        }
        public ClientMaster getClientById(int id)
        {
            var client = _clientMasterRepository.Get(cl => cl.ClientId == id);
            return client;
        }
        public IEnumerable<ClientMaster> GetClientNames(string name)
        {
            var namelist = _clientMasterRepository.GetMany(clname => clname.ClientName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(clname => clname.ClientName);
            return namelist;
        }

        public IEnumerable<ClientMaster> ValidateName(string clientname)
        {
            var clnamelist = _clientMasterRepository.GetMany(cl => cl.ClientName == clientname);
            return clnamelist;
        }

        public IEnumerable<ClientMaster> GetAllClients()
        {
            var list = _clientMasterRepository.GetAll();
            return list;
        }

        public IEnumerable<ClientMaster> GetActiveAndMaharashtraClients(string name)
        {
            var list = _clientMasterRepository.GetMany(cl => cl.ClientName.ToLower().StartsWith(name.ToLower()) && cl.State == "Maharashtra" && cl.Status == "Active");
            return list;
        }

        public IEnumerable<ClientMaster> GetActiveClients()
        {
            var details = _clientMasterRepository.GetMany(cl => cl.Status == "Active");
            return details;
        }
    }
}
