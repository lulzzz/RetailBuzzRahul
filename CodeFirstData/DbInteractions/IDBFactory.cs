using System;

namespace CodeFirstData.DBInteractions
{
    public interface IDBFactory : IDisposable
    {
        CodeFirstContext Get();
    }
}
