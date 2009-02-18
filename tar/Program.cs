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
                using (TarWriter legacyTar = new TarWriter(archUsTar))
                {
                    for (int i = 1; i < args.Length; ++i)
                    {
                        legacyTar.Write(args[i]);
                    }
                }
            }

            Console.WriteLine("Examine tar file: {0}", args[0]);
            using (FileStream examiner = File.OpenRead(args[0]))
            {
                TarReader tar = new TarReader(examiner);
                while (tar.MoveNext(true))
                {
                    Console.WriteLine("File: {0}, Owner: {1}", tar.FileInfo.FileName, tar.FileInfo.UserName);
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