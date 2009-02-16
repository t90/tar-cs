using System.IO;
using System.Net;

namespace tar_cs
{
    public class TarReader
    {
        private readonly Stream inStream;

        public TarReader(Stream tarredData)
        {
            inStream = tarredData;
        }

        public void ReadAll(string destPath)
        {

        }

        public bool Read(out string fileName, Stream data)
        {
            fileName = "";
            return false;
        }

        public virtual bool Read(out string fileName, ReadDataDelegate reader)
        {
            ReadHeader();

            fileName = "";
            return false;
        }

        private void ReadHeader()
        {
            
//            inStream.Read()
        }
    }
}