using System;
using System.IO;
using tar_cs;

namespace tar
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("USAGE: ArchiveMaker fileName.tar <fileToAdd.ext> [. more files..]");
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