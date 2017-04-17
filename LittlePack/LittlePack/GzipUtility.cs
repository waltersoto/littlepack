
using System.IO;
using System.IO.Compression;

namespace LittlePack {
    internal static class GzipUtility {
        public static byte[] GzipIt(this byte[] data) {
            using (var compressed = new MemoryStream()) {
                using (var gz = new GZipStream(compressed, CompressionMode.Compress)) {
                    gz.Write(data, 0, data.Length);
                }
                return compressed.ToArray();
            }
        }

        public static byte[] UnGzipIt(this byte[] data) {
            using (var gz = new GZipStream(new MemoryStream(data), CompressionMode.Decompress)) {
                using (var decompressed = new MemoryStream()) {
                    gz.CopyTo(decompressed);
                    return decompressed.ToArray();
                }
            }
        }

    }
}
