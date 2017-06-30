using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IColorCodeService
    {
        IEnumerable<ColorCode> getAllColorCode();
        ColorCode GetColorCodebyId(int id);
        ColorCode getColorNameByCode(string code);
        ColorCode GetLastColor();
        IEnumerable<ColorCode> GetColorList(string term);
        IEnumerable<ColorCode> GetColorName(string ColorName);
        void Create(ColorCode Color);
        void Update(ColorCode Color);
        string getColorName(string colorname);
        ColorCode GetcolorcodebyName(string colorname);
    }
}
