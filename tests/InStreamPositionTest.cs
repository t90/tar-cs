using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using tar_cs;

namespace tests
{
    [TestFixture]
    public class InStreamPositionTest
    {
        [Test]
        public void IssueNo5Test()
        {
            var fileMap = new List<KeyValuePair<string, byte[]>>()
            {
                new KeyValuePair<string, byte[]>("test1", Encoding.ASCII.GetBytes("test1")),
                new KeyValuePair<string, byte[]>("test2", Encoding.ASCII.GetBytes("test2")),
                new KeyValuePair<string, byte[]>("test3", Encoding.ASCII.GetBytes("test3")),
                new KeyValuePair<string, byte[]>("test4", Encoding.ASCII.GetBytes("test4")),
                new KeyValuePair<string, byte[]>("test5", Encoding.ASCII.GetBytes("test5")),
                new KeyValuePair<string, byte[]>("test6", File.ReadAllBytes(Assembly.GetExecutingAssembly().Location)),
            };

            using (var fs = File.Create(Path.GetTempFileName()))
            {
                using (var outStream = new GZipStream(fs, CompressionMode.Compress))
                {
                    using (var writer = new TarWriter(fs))
                    {
                        foreach (var entry in fileMap)
                        {
                            using (var fileStream = new MemoryStream(entry.Value))
                            {
                                writer.Write(fileStream, fileStream.Length, entry.Key);
                            }
                        }
                    }
                }
            }            
        }

    }
}
