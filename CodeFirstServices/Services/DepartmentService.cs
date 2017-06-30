using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class DepartmentService:IDepartmentService
    {
         private readonly IDepartmentRepository _deprepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IDepartmentRepository depRepository, IUnitOfWork unitOfWork)
        {
            this._deprepository = depRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<Department> getAllDepartments()
        {
            var departments = _deprepository.GetAll();
            return departments;

        }

    }
}
