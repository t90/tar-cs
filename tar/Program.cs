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
            using (FileStream archUsTar = File.Create(args[0]))
            {
                using (TarWriter tar = new TarWriter(archUsTar))
                {
                    for (int i = 1; i < args.Length; ++i)
                    {
                        tar.Write(args[i]);
                    }
                }
            }
            using (FileStream unarchFile = File.OpenRead(args[0]))
            {
                TarReader reader = new TarReader(unarchFile);
                reader.ReadToEnd("out_dir\\data");
            }
        }
    }
}