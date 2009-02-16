using System;
using System.Text;
using tar_cs;

namespace tar_cs
{
    internal class TarHeader : ITarHeader
    {
        private readonly byte[] buffer = new byte[512];
        private long headerChecksum;

        public TarHeader()
        {
            // Default values
            Mode = 511; // 0777 dec
            UserId = 61; // 101 dec
            GroupId = 61; // 101 dec
        }

        private string name;

        public virtual string Name
        {
            get
            {
                return name;
            } 
            set
            {
                if(value.Length > 100)
                {
                    throw new TarException("A file name can not be more than 100 chars long");
                }
                name = value;
            }
        }
        public int Mode { get; set; }

        public string ModeString
        {
            get { return AddChars(Convert.ToString(Mode, 8), 7, '0', true); }
        }

        public int UserId { get; set; }

        public string UserIdString
        {
            get { return AddChars(Convert.ToString(UserId, 8), 7, '0', true); }
        }

        public int GroupId { get; set; }

        public string GroupIdString
        {
            get { return AddChars(Convert.ToString(GroupId, 8), 7, '0', true); }
        }

        public long SizeInBytes { get; set; }

        public string SizeString
        {
            get { return AddChars(Convert.ToString(SizeInBytes, 8), 11, '0', true); }
        }

        public DateTime LastModification { get; set; }

        public string LastModificationString
        {
            get
            {
                return AddChars(
                    ((long) (LastModification - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString(), 11, '0',
                    true);
            }
        }

        public string HeaderChecksumString
        {
            get { return AddChars(Convert.ToString(headerChecksum, 8), 6, '0', true); }
        }


        public virtual int HeaderSize
        {
            get { return 512; }
        }

        private static string AddChars(string str, int num, char ch, bool isLeading)
        {
            int neededZeroes = num - str.Length;
            while (neededZeroes > 0)
            {
                if (isLeading)
                    str = ch + str;
                else
                    str = str + ch;
                --neededZeroes;
            }
            return str;
        }


        public virtual byte[] GetHeaderValue()
        {
            // Clean old values
            int i = 0;
            while (i < 512)
            {
                buffer[i] = 0;
                ++i;
            }

            if (string.IsNullOrEmpty(Name)) throw new TarException("FileName can not be empty.");
            if (Name.Length >= 100) throw new TarException("FileName is too long. It must be less than 100 bytes.");

            // Fill header
            Encoding.ASCII.GetBytes(AddChars(Name, 100, '\0', false)).CopyTo(buffer, 0);
            Encoding.ASCII.GetBytes(ModeString).CopyTo(buffer, 100);
            Encoding.ASCII.GetBytes(UserIdString).CopyTo(buffer, 108);
            Encoding.ASCII.GetBytes(GroupIdString).CopyTo(buffer, 116);
            Encoding.ASCII.GetBytes(SizeString).CopyTo(buffer, 124);
            Encoding.ASCII.GetBytes(LastModificationString).CopyTo(buffer, 136);

            RecalculateChecksum(buffer);

            // Write checksum
            Encoding.ASCII.GetBytes(HeaderChecksumString).CopyTo(buffer, 148);

            return buffer;
        }

        protected virtual void RecalculateChecksum(byte[] buf)
        {
// Set default value for checksum. That is 8 spaces.
            Encoding.ASCII.GetBytes("        ").CopyTo(buf, 148);

            // Calculate checksum
            headerChecksum = 0;
            foreach (byte b in buf)
            {
                headerChecksum += b;
            }
        }
    }
}