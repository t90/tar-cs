using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using tar_cs;

namespace tests
{
    [TestFixture]
    public class SampleFilesTest
    {
        [Test]
        public void Issue10Test()
        {
            using (var f = File.OpenRead("campsite-3.3.4.tar.gz"))
            {
                using (var g = new GZipStream(f, CompressionMode.Decompress))
                {
                    var r = new TarReader(g);
                    r.ReadToEnd(".");
                }
            }
        }
    }
}
