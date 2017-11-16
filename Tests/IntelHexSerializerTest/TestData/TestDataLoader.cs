using System.IO;

namespace IntelHexSerializerTest.TestData
{
    public static class TestDataLoader
    {
        public const string HexFileWithStartOffset = "hexFileWithStartOffset.hex";
        public const string HexFileWithStartOffset2 = "hexFileWithStartOffset2.hex";
        public const string HexFileWithoutOffsetString = "hexFileWithoutOffset.hex";
        public const string HexFileWithDefaultEnding = "hexFileWithDefaultEnding.hex";
        public const string HexFileWithDefaultEnding2 = "hexFileWithDefaultEnding2.hex";
        public const string HexFileWithTwoOffests = "hexFileWithTwoOffsets.hex";
        public const string HexFileWithOverlap = "hexFileWithOverlap.hex";

        private static Stream FileStream(string filename)
        {
            var assembly = typeof(TestDataLoader).Assembly;
            var fullFilePath = $"IntelHexSerializerTest.TestData.{filename}";
            return assembly.GetManifestResourceStream(fullFilePath);
        }

        public static string GetString(string filename)
        {
            using (var reader = new StreamReader(FileStream(filename)))
                return reader.ReadToEnd();
        }
    }
}
