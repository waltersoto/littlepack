# littlepack
File archiving library for .NET 4.0

LittlePack allows you to archive multiple file into one compressed package (GZip).

Creating a package:

```csharp
  var man = new Packer();
           
  man.Records.Add(new Record { FileName = "file1.png", Data = File.ReadAllBytes(@"C:\Data\file1.png") });
  man.Records.Add(new Record { FileName = "file2.png", Data = File.ReadAllBytes(@"C:\Data\file1.png") } );
  man.Records.Add(new Record {FileName = "file3.png", Data = File.ReadAllBytes(@"C:\Data\file3.png") });
  
  byte[] package = man.Pack();

  File.WriteAllBytes(@"C:\Temp\package.pack", package);
```

Unpacking:

```csharp
 List<Record> files = Packer.Unpack(File.ReadAllBytes(@"C:\Temp\package.pack"));

Packer.Unpack(File.ReadAllBytes(@"C:\Temp\package.pack")).SaveTo(@"C:\Temp\file\");
```
