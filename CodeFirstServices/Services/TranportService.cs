using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class TranportService:ITransportService
    {
        private readonly ITransportMasterRepository _TransportMasterRepository;
        private readonly IUnitOfWork _unityOfWork;
          public TranportService(ITransportMasterRepository TransportMasterRepository, IUnitOfWork unityOfWork)
          {
              this._TransportMasterRepository = TransportMasterRepository;
              this._unityOfWork = unityOfWork;            
          }

           public IEnumerable<TransportMaster> GetTransportByMode(string transport)
          {
              var transportList = _TransportMasterRepository.GetMany(trans => trans.ModeOfTransport == transport && trans.Status == "Active");
              return transportList;
          }

          public TransportMaster getLastInsertedTransport()
          {
              var transport = _TransportMasterRepository.GetAll().LastOrDefault();
              return transport;
          }

          public TransportMaster getById(int id)
          {
              var transport = _TransportMasterRepository.GetById(id);
              return transport;
          }

          public void CreateTransport(TransportMaster Transport)
          {
              _TransportMasterRepository.Add(Transport);
              _unityOfWork.Commit();
          }

          public void UpdateTransport(TransportMaster Transport)
          {
              _TransportMasterRepository.Update(Transport);
              _unityOfWork.Commit();
          }

          public IEnumerable<TransportMaster> GetEmail(string mail)
          {
              var transportList = _TransportMasterRepository.GetMany(trans => trans.Email == mail);
              return transportList;
          }

          public IEnumerable<TransportMaster> GetTranportNames(string name)
          {
              var namelist = _TransportMasterRepository.GetMany(trans => trans.TransportName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(trans => trans.TransportName);
              return namelist;
          }

          public IEnumerable<TransportMaster> ValidateName(string transportname)
          {
              var transportList = _TransportMasterRepository.GetMany(trans => trans.TransportName == transportname);
              return transportList;
          }

          public TransportMaster GetByName(string name)
          {
              var details = _TransportMasterRepository.Get(trans => trans.TransportName == name);
              return details;
          }

          public IEnumerable<TransportMaster> GetAllTransport()
          {
              var transportList = _TransportMasterRepository.GetAll();
              return transportList;
          }

    }
}
