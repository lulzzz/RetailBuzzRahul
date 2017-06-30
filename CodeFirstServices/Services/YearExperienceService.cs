using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
   public class YearExperienceService :IYearExperienceService
    {
       private readonly IYearExperienceRepository _YearExperienceRepository;
       private readonly IUnitOfWork _UnitOfWork;
       public YearExperienceService(IYearExperienceRepository YearExperienceRepository, IUnitOfWork UnitOfWork)
       {
           this._YearExperienceRepository= YearExperienceRepository;
           this._UnitOfWork= UnitOfWork;
       }
       public IEnumerable<YearExperience> GetYearExp()
       {
           var Yr = _YearExperienceRepository.GetAll();
           return Yr;
       }
    }
}
