using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;

namespace CodeFirstServices.Services
{
    public class JobWorkerService : IJobWorkerService
    {
        private readonly IJobWorkerRepository _JobWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobWorkerService(IJobWorkerRepository JobWorkerRepository, IUnitOfWork unitOfWork)
        {
            this._JobWorkerRepository = JobWorkerRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(JobWorker worker)
        {
            _JobWorkerRepository.Add(worker);
            _unitOfWork.Commit();
        }

        public JobWorker GetLastRow()
        {
            var row = _JobWorkerRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<JobWorker> GetEmail(string mail)
        {
            var data = _JobWorkerRepository.GetMany(emp => emp.Email == mail);
            return data;
        }

        public JobWorker GetDetailsById(int id)
        {
            var data = _JobWorkerRepository.GetById(id);
            return data;
        }

        public IEnumerable<JobWorker> GetAll()
        {
            var data = _JobWorkerRepository.GetAll();
            return data;
        }

        public JobWorker GetDetailsByName(string name)
        {
            var data = _JobWorkerRepository.Get(d => d.Name == name);
            return data;
        }

        public IEnumerable<JobWorker> GetJobWorkerNames(string term)
        {
            var details = _JobWorkerRepository.GetMany(m => m.Name.ToLower().StartsWith(term.ToLower()) && m.Status == "Active").OrderBy(m => m.Name);
            return details;
        }

        public IEnumerable<JobWorker> GetActiveJobWorkers()
        {
            var data = _JobWorkerRepository.GetMany(emp => emp.Status == "Active");
            return data;
        }
    }
}
