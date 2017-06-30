using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
   public  class JobWorkOutwardToClientService : IJobWorkOutwardToClientService
    {
        private readonly IJobWorkOutwardToClientRepository _JobWorkOutwardToClientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobWorkOutwardToClientService(IJobWorkOutwardToClientRepository JobWorkOutwardToClientRepository, IUnitOfWork unitOfWork)
        {
            this._JobWorkOutwardToClientRepository = JobWorkOutwardToClientRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(JobWorkOutwardToClient data)
        {
            _JobWorkOutwardToClientRepository.Add(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<JobWorkOutwardToClient> GetActiveOutwardToClients()
        {
            var data = _JobWorkOutwardToClientRepository.GetMany(emp => emp.Status == "Active");
            return data;
        }
    }
}
