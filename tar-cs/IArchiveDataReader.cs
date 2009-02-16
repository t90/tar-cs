namespace tar_cs
{
    public interface IArchiveDataReader
    {
        bool CanRead { get;}
        byte[] Read(out int count);
    }

    public delegate void ReadDataDelegate(IArchiveDataReader reader);
}