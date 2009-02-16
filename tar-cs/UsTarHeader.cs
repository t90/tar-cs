using System;
using System.Net;
using System.Text;

namespace tar_cs
{
    /// <summary>
    /// UsTar header implementation.
    /// </summary>
    internal class UsTarHeader : TarHeader
    {
        private const string magic = "ustar";
        private const string version = "  ";
        private string groupName;

        private string namePrefix = string.Empty;
        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                if (value.Length > 32)
                {
                    throw new TarException("user name can not be longer than 32 chars");
                }
                userName = value;
            }
        }

        public string GroupName
        {
            get { return groupName; }
            set
            {
                if (value.Length > 32)
                {
                    throw new TarException("group name can not be longer than 32 chars");
                }
                groupName = value;
            }
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                if (value.Length > 100)
                {
                    if (value.Length > 255)
                    {
                        throw new TarException("UsTar fileName can not be longer thatn 255 chars");
                    }
                    int position = value.Length - 100;

                    // Find first path separator in the remaining 100 chars of the file name
                    while (!IsPathSeparator(value[position]))
                    {
                        ++position;
                        if (position == value.Length)
                        {
                            break;
                        }
                    }
                    if (position == value.Length)
                        position = value.Length - 100;
                    namePrefix = value.Substring(0, position);
                    base.Name = value.Substring(position, value.Length - position);
                }
                else
                {
                    base.Name = value;
                }
            }
        }

        private static bool IsPathSeparator(char ch)
        {
            return (ch == '\\' || ch == '/' || ch == '|'); // All the path separators I ever met.
        }

        public override byte[] GetHeaderValue()
        {
            byte[] header = base.GetHeaderValue();

            Encoding.ASCII.GetBytes(magic).CopyTo(header, 0x101); // Mark header as ustar
            Encoding.ASCII.GetBytes(version).CopyTo(header, 0x106);
            Encoding.ASCII.GetBytes(UserName).CopyTo(header, 0x109);
            Encoding.ASCII.GetBytes(GroupName).CopyTo(header, 0x129);
            Encoding.ASCII.GetBytes(namePrefix).CopyTo(header, 347);

            if (SizeInBytes >= 0x1FFFFFFFF)
            {
                byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(SizeInBytes));
                SetMarker(AlignTo12(bytes)).CopyTo(header, 124);
            }

            RecalculateChecksum(header);
            Encoding.ASCII.GetBytes(HeaderChecksumString).CopyTo(header, 148);
            return header;
        }

        private static byte[] SetMarker(byte[] bytes)
        {
            bytes[0] |= 0x80;
            return bytes;
        }

        private static byte[] AlignTo12(byte[] bytes)
        {
            var retVal = new byte[12];
            bytes.CopyTo(retVal, 12 - bytes.Length);
            return retVal;
        }
    }
}