using System.IO;

namespace tar_cs
{
    internal class DataReader : IArchiveDataReader
    {
        private readonly byte[] buffer = new byte[BLOCK_SIZE];
        private bool canRead = true;
        private long remaining;
        private readonly Stream stream;
        const int BLOCK_SIZE = 512;

        public DataReader(Stream data, long sizeInBytes)
        {
            stream = data;
            remaining = sizeInBytes;
        }

        #region IArchiveDataReader Members

        public bool CanRead
        {
            get { return canRead; }
        }

        public byte[] Read(out int count)
        {
            int read = stream.Read(buffer,0,BLOCK_SIZE);
            if (read < 0)
            {
                canRead = false;
                count = -1;
                return null;
            }
            remaining -= read;
            if (remaining < 0)
            {
                canRead = false;
                count = (int)(BLOCK_SIZE - remaining);
            }
            else
                count = read;
            return buffer;
        }

        #endregion
    }


}