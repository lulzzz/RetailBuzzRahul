using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
   public class MonthExperienceService :IMonthExperienceService
    {
       private readonly IMonthExperienceRepository _MonthExperienceRepository;
       private readonly IUnitOfWork _UnitOfWork;
       public MonthExperienceService(IMonthExperienceRepository MonthExperienceRepository, IUnitOfWork UnitOfWork)
       {

           this._MonthExperienceRepository = MonthExperienceRepository;
           this._UnitOfWork = UnitOfWork;
       }
       public IEnumerable<MonthExperience> GetMonthExp()
       {
           var Month = _MonthExperienceRepository.GetAll();
           return Month;
       }
    }
}
