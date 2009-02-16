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

            using(var archUsTar = File.Create(args[0]))
            {
                using(var tar = new TarWriter(archUsTar))
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