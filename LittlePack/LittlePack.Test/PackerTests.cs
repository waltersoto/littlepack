
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LittlePack.Test {
    [TestClass]
    public class PackerTests {

        private byte[] file1 = new byte[] { 116, 101, 115, 116, 32, 116, 101, 115, 116, 32, 116, 101, 115, 116 };
        private byte[] file2 = new byte[] { 116, 101, 115, 116, 50, 32, 116, 101, 115, 116, 50, 32, 116, 101, 115, 116, 50 };
        private byte[] file3 = new byte[] { 116, 101, 115, 116, 51, 32, 116, 101, 115, 116, 50, 32, 116, 101, 115, 116, 51 };

        private byte[] packagedFiles = new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 19, 100, 96, 96, 72, 203, 204, 73, 53, 212, 1, 145, 70, 96, 210, 216, 6, 40, 200, 7, 196, 37, 169, 197, 37, 10, 112, 66, 16, 42, 98, 164, 128, 68, 194, 4, 141, 145, 4, 141, 1, 121, 51, 71, 4, 85, 0, 0, 0 };

        [TestMethod]
        public void Packer_Pack() {
                        
            var packer = new Packer(new List<Record>(3) {
                new Record { Data = file1, FileName = "file1" },
                new Record { Data = file2, FileName = "file2" },
                new Record { Data = file3, FileName = "file3" }
            });

            var package = packer.Pack();
            CollectionAssert.AreEqual(packagedFiles, package);

        }

        [TestMethod]
        public void Packer_Unpack() {
            var files = Packer.Unpack(packagedFiles);
            CollectionAssert.AreEqual(file1, files[0].Data);
            CollectionAssert.AreEqual(file2, files[1].Data);
            CollectionAssert.AreEqual(file3, files[2].Data);
        }

    }
}
