using System;
using System.Collections.Generic;
using System.IO;
using tar_cs;

namespace tar
{
    internal class Program
    {
        private static IEnumerator<byte []> NextDataBlock(long dataSize)
        {
            byte[] buffer = new byte[512];
            var random = new Random(DateTime.Now.Millisecond);
            while(dataSize > 512)
            {
                random.NextBytes(buffer);
                dataSize -= 512;
                yield return buffer;
            }

        }

        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("USAGE: ArchiveMaker fileName.tar <fileToAdd.ext> [. more files..]");
                return;
            }

            if(args.Length == 2 && args[0] == "--test-big")
            {
                using(var file = File.Create(args[1]))
                using(var tar = new UsTarWriter(file))
                {
                    IEnumerator<byte[]> i = NextDataBlock(9255295864);

                    tar.Write("bigFile.bin", 9255295864, "v", "v", 0777, DateTime.Now, delegate(IDataWriter writer)
                    {
//                        i.MoveNext();
//                        byte[] current = i.Current;
//                        writer.Write(current, current.Length);
                    });
                }
                
                return;
            }

            using (FileStream archiveFile = File.Create(args[0]))
            {
                using (TarWriter tar = new TarWriter(archiveFile))
                {
                    for (int i = 1; i < args.Length; ++i)
                    {
                        tar.Write(args[i]);
                    }
                    MemoryStream stream = new MemoryStream();
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine("This archive created with tar-cs.");
                    writer.Flush();
                    stream.Position = 0;
                    tar.Write(stream, stream.Length, "README.TXT");
                }
            }

            using(var archUsTar = File.Create(args[0]+".ustar.tar"))
            {
                using(var tar = new UsTarWriter(archUsTar))
                {
                    for (int i = 1; i < args.Length; ++i)
                    {
                        tar.Write(args[i]);
                    }
                }
            }

        }
    }
}