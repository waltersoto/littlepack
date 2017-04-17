/*
The MIT License (MIT)

Copyright (c) 2015 Walter M. Soto Reyes

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.    
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittlePack {
    public class Packer {

        private const string MANIFEST_SPLIT = ",";

        public Packer() : this(new List<Record>()) { }

        public Packer(List<Record> records) {
            Records = records;
        }

        public static List<Record> Unpack(byte[] package) {
            var result = new List<Record>();
            package = package.UnGzipIt();
            var temp = UnpackIt(package);
            if (temp.Count != 2) return result;
            var manifest = Encoding.ASCII.GetString(temp[0]);
            var files = manifest.Split(MANIFEST_SPLIT[0]);
            var data = UnpackIt(temp[1]);

            if (files.Length != data.Count) return result;

            result.AddRange(files.Select((fileName, index) => new Record {
                FileName = fileName, Data = data[index]
            }));

            return result;
        }

        private static List<byte[]> UnpackIt(byte[] package) {

            var len = package.Length;
            const int Header = 4;
            var position = 0;
            var list = new List<byte[]>();

            if (len <= 4) return list;
            while (len > 0) {
                var head = package.Skip(position).Take(Header).ToArray();
                var size = GetSize(head);
                var data = package.Skip(position + Header).Take(size).ToArray();
                list.Add(data);
                position += (Header + size);
                len -= (Header + size);
            }

            return list;
        }

        public byte[] Pack() {
            return PackIt(new List<byte[]> { Manifest(), PackIt(Records.Select(m => m.Data).ToList()) }).GzipIt();
        }

        private byte[] Manifest() {
            if (Records.Count < 1) return null;
            var man = string.Join(MANIFEST_SPLIT, Records.Select(m => m.FileName.Replace(MANIFEST_SPLIT, "")));
            return Encoding.ASCII.GetBytes(man);
        }
        
        private static byte[] PackIt(IEnumerable<byte[]> records) {
            var files = new List<byte[]>();

            foreach (var t in records) {
                var size = GetSizeInBytes(t.Length);
                files.Add(size);
                files.Add(t);
            }

            return files.SelectMany(m => m).ToArray();

        }

        private static int GetSize(byte[] size) => BitConverter.ToInt32(size, 0);
        
        private static byte[] GetSizeInBytes(int size) => BitConverter.GetBytes(size);

        public List<Record> Records { set; get; }

    }
}
