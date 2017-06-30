using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class ColorCodeService : IColorCodeService
    {
        private readonly IColorCodeRepository _colorCodeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ColorCodeService(IColorCodeRepository colorCodeRepository, IUnitOfWork unitOfWork)
        {
            this._colorCodeRepository= colorCodeRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<ColorCode> getAllColorCode()
        {
            var colorData = _colorCodeRepository.GetAll();
            return colorData;
        }

        public ColorCode GetColorCodebyId(int id)
        {
            var colorData = _colorCodeRepository.GetById(id);
            return colorData;
        }
        public ColorCode getColorNameByCode(string code)
        {
            var colorData = _colorCodeRepository.Get(cc => cc.colorCode == code);
            return colorData;
        }

        public ColorCode GetLastColor()
        {
            var details = _colorCodeRepository.GetAll().LastOrDefault();
            return details;
        }

        public IEnumerable<ColorCode> GetColorList(string term)
        {
            var list = _colorCodeRepository.GetMany(c => c.colorName.ToLower().StartsWith(term.ToString().ToLower())).OrderBy(s => s.colorName);
            return list;
        }

        public IEnumerable<ColorCode> GetColorName(string ColorName)
        {
            var details = _colorCodeRepository.GetMany(c => c.colorName == ColorName);
            return details;
        }

        public void Create(ColorCode Color)
        {
            _colorCodeRepository.Add(Color);
            _unitOfWork.Commit();
        }

        public void Update(ColorCode Color)
        {
            _colorCodeRepository.Update(Color);
            _unitOfWork.Commit();
        }

        public string getColorName(string colorname)
        {
            var data = _colorCodeRepository.Get(c => c.colorName == colorname);
            return data.colorName;
        }

        public ColorCode GetcolorcodebyName(string colorname)
        {
            var colorcode = _colorCodeRepository.Get(cc => cc.colorName == colorname);
            return colorcode;
        }
    }
}
