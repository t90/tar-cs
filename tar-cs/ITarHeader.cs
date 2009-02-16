using System;

namespace tar_cs
{
    public interface ITarHeader
    {
        string Name { get; set; }
        int Mode { get; set; }
        int UserId { get; set; }
        int GroupId { get; set; }
        long SizeInBytes { get; set; }
        DateTime LastModification { get; set; }
        int HeaderSize { get; }
        byte[] GetHeaderValue();
        byte[] GetBytes();
    }
}