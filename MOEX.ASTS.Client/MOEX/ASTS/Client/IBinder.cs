namespace MOEX.ASTS.Client
{
    public interface IBinder
    {
        ITarget Detect(Meta.Message source);
    }
}

