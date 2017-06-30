using CodeFirstEntities;
using System.Data.Entity;

namespace CodeFirstData.DBInteractions
{
public class DBFactory : Disposable, IDBFactory
{

    public DBFactory()
    {
        Database.SetInitializer<CodeFirstContext>(null);
    }
    private CodeFirstContext dataContext;
    public CodeFirstContext Get()
    {
        return dataContext ?? (dataContext = new CodeFirstContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
