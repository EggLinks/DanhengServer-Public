namespace EggLink.DanhengServer.Data;

public abstract class ExcelResource
{
    public abstract int GetId();

    public virtual void Loaded()
    {
    }

    public virtual void Finalized()
    {
    }

    public virtual void AfterAllDone()
    {
    }
}