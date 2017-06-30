using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ITypeOfMaterialService
    {
        void AddTypeOfMaterial(TypeOfMaterial typeOfMaterial);
        IEnumerable<TypeOfMaterial> GetMaterialList();
        TypeOfMaterial GetMaterialDetailById(int id);
        IEnumerable<TypeOfMaterial> GetMaterialforList(string Material);
        TypeOfMaterial GetMaterialLast();
        void UpdateTypeOfMaterial(TypeOfMaterial typeOfMaterial);
        IEnumerable<TypeOfMaterial> getAllTypeOfMaterial();
        TypeOfMaterial GetTypeOfMaterialByCode(string code);
        string GetNameByShortName(string shortname);
        IEnumerable<TypeOfMaterial> GetMaterialNames(string name);
        TypeOfMaterial GetMaterialName(string name);
    }
}
