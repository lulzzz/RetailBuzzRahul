using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;

namespace CodeFirstServices.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ModuleService(IModuleRepository moduleRepository, IUnitOfWork unitOfWork)
        {
            this._moduleRepository = moduleRepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateModule(Module module)
        {
            _moduleRepository.Add(module);
            _unitOfWork.Commit();
        }

        public IEnumerable<Module> getAllModules()
        {
            var modulename = _moduleRepository.GetAll();
            return modulename;
        }
        public int getModuleIdByModulename(string name)
        {
            var id = _moduleRepository.Get(ml => ml.ModuleName == name);
            return id.Id;
        }

        public Module GetLastRow()
        {
            var data = _moduleRepository.GetAll().LastOrDefault();
            return data;
        }

        public Module GetDataByModuleId(int id)
        {
            var data = _moduleRepository.Get(d => d.Id == id);
            return data;
        }

    }
}
