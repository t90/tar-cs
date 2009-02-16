using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tar_cs
{
    public class UsTarWriter : TarWriter
    {
        private UsTarHeader header;

        public UsTarWriter(Stream writeStream) : base(writeStream)
        {
        }

        protected override ITarHeader GetTarHeader()
        {
            if(header == null)
                header = new UsTarHeader();
            return header;
        }

        public void Write(Stream data, long dataSizeInBytes, string fileName, string userId, string groupId, int mode, DateTime lastModificationTime)
        {
            DirtyHeaderMethod(userId, groupId);
            Write(data, dataSizeInBytes, fileName, userId.GetHashCode(), groupId.GetHashCode(), mode, lastModificationTime);
        }

        //@TODO Replace all the header stuff with smth more sane. I need to think about it.
        private void DirtyHeaderMethod(string userId, string groupId)
        {
            ITarHeader iHeader = GetTarHeader();
            if(iHeader is UsTarHeader)
            {
                var h = (UsTarHeader) iHeader;
                h.UserName = userId;
                h.GroupName = groupId;
            }
        }

        public override void Write(Stream data, long dataSizeInBytes, string name, int userId, int groupId, int mode, DateTime lastModificationTime)
        {
            DirtyHeaderMethod(Convert.ToString(userId,8), Convert.ToString(userId,8));
            base.Write(data, dataSizeInBytes, name, userId, groupId, mode, lastModificationTime);
        }

    }
}
