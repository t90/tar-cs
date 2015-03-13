### News ###

Updated featured source to [tar-cs\_20140118](https://tar-cs.googlecode.com/files/tar-cs_20140118.zip) including most recent source from version control.

Switched version control from SVN to GIT.

### Pre ###

If you ever had a need to send multiple files to a client using your C# code. If want your client to be able to extract those files with some kind of widely recognized application. If you did not want to make your code dependencies much much much bigger. If yes then you had the same requirement I do.

### Result ###
I came out with this small library. This library compatible with mono and .NET framework 2.0 and higher. You can use it to create regular ".tar" files or to create "tar.gz" archives that are widely used across Linux/Unix world and recognized by shell extensions like "WinZip" or "WinRar".

### Benefits ###
This is the library enables your to [read](http://code.google.com/p/tar-cs/wiki/TarReader) and [write](http://code.google.com/p/tar-cs/wiki/TarWriter) tar archives from your .NET/mono code.

Here are the
### Examples ###

First of all you should reference tar-cs.dll and create tar-cs.TarWriter object:

```

var tar = new TarWriter(OutStream);

```

OutStream is an object of any System.IO.Stream inherited class that supports write operation. That is a destination for you tar-ed data. It's your destination file in most cases.

As soon as TarWriter implements IDisposable, it's good idea to use keyword **using** while creating TarWriter, so the object will be closed right after you left the corresponding context:

```

using(var tar = new TarWriter(OutStream))
{
// ... context ...
}

```

Next thing we plan to do is to add some files to the archive. Let's go:

```

using(var tar = new TarWriter(OutStream))
{
    tar.Write("myFileToBeAdded.txt");
}

```

This will write your file contents to the tar archive.

You can also use overloaded Write methods

  * [Write(string fileName)](http://code.google.com/p/tar-cs/wiki/WriteString)
  * [Write(FileStream file)](http://code.google.com/p/tar-cs/wiki/WriteFileStream)
  * [Write(Stream data, long dataSizeInBytes, string name)](http://code.google.com/p/tar-cs/wiki/WriteStream)
  * [Write(Stream data, long dataSizeInBytes, string name, int userId, int groupId, int  ode, DateTime lastModificationTime)](http://code.google.com/p/tar-cs/wiki/WriteStreamAndParameters)

### Reading archive ###

If you want to extract archive someone created for you, then you should use TarReader. Here is an example:

```

using (FileStream unarchFile = File.OpenRead(args[0]))
{
    TarReader reader = new TarReader(unarchFile);
    reader.ReadToEnd("out_dir\\data");
}

```

It's the easiest way. ReadToEnd just extracts all the files from your archive to a specified folder. If you want more
control on extraction process, then you should use MoveNext and [Read](Read.md) methods:

```
            
TarReader reader = new TarReader(unarchFile);
while (reader.MoveNext(false))
{
    var path = reader.FileInfo.FileName;
    using (FileStream file = File.Create(path))
    {
        Read(file);
    }
}

```

### When you don't need the real data ###

If you want to examine names of the files that are in your archive but not extract files here is the example:

```

TarReader reader = new TarReader(unarchFile);
while (reader.MoveNext(true))
{
    Console.WriteLine("FileName is {0}, owner is {1}", reader.FileInfo.FileName, reader.FileInfo.UserName);
}

```

MoveNext(true) means that you don't need the data and want just skip it.

### More complex example ###
If you want to create tar.gz. The file where data is compressed. No problems at all:

```

using(var outFile = File.Create("myOutFile.tar.gz"))
{
    using(var outStream = new GZipStream(outFile ,CompressionMode.Compress))
    {
        using(var writer = new TarWriter(outStream))
        {
            writer.Write("myFileToAdd.txt");
        }
    }
}

```

### What is _tar_? ###

  * http://en.wikipedia.org/wiki/Tar_(file_format)

  * http://www.gnu.org/software/tar/

  * http://www.linfo.org/tar.html

### License? ###

BSD License. See [Copying](Copying.md)