
using System.Collections.Generic;
using System.IO;

namespace LittlePack
{
    public static class LittlePackExtensions
    {
        private static string FormatDir(string dir)
        {
            if (dir.EndsWith(@"\")) return dir;
            dir = dir.TrimEnd('/');
            dir = dir + @"\";
            return dir;
        }

      

        public static void SaveTo(this Record record, string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            directoryPath = FormatDir(directoryPath);
            File.WriteAllBytes(string.Concat(directoryPath, record.FileName), record.Data);
        }

        public static void SaveTo(this List<Record> records, string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            directoryPath = FormatDir(directoryPath);

            foreach (var record in records)
            {
                File.WriteAllBytes(string.Concat(directoryPath,record.FileName),record.Data);
            }
        }

        public static byte Bcc(this byte[] data)
        {
            byte bcc = 0;

            if (data == null || data.Length <= 0) return bcc;
            for (var i = 1; i < data.Length; i++)
            {
                bcc ^= data[i];
            }

            return bcc;
        }

    }
}
