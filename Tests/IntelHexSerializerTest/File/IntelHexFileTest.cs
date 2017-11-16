using IntelHexSerializer.File;
using IntelHexSerializerTest.TestData;
using NUnit.Framework;

namespace IntelHexSerializerTest.File
{
    [TestFixture]
    public class IntelHexFileTest
    {
        [TestCase(TestDataLoader.HexFileWithStartOffset, 0x00010000)]
        [TestCase(TestDataLoader.HexFileWithStartOffset2, 0x00010000)]
        [TestCase(TestDataLoader.HexFileWithTwoOffests, 0x00010000)]
        [TestCase(TestDataLoader.HexFileWithoutOffsetString, 0x00000000)]
        [TestCase(TestDataLoader.HexFileWithDefaultEnding, 0x00000000)]
        [TestCase(TestDataLoader.HexFileWithDefaultEnding2, 0x00000000)]
        [TestCase(TestDataLoader.HexFileWithOverlap, 0x00000000)]
        [TestCase(TestDataLoader.HexFileWithOverlap, 0x00010000)]
        public void SerializeDeserializeShouldBeTheSame(string testHexFile, long baseAddress)
        {
            var hexString = TestDataLoader.GetString(testHexFile);

            var intelHexFile = IntelHexFile.CreateFrom(hexString, (int)baseAddress);
            var intelHexFile2 = IntelHexFile.CreateFrom(intelHexFile.BinaryData, (int)baseAddress);

            Assert.AreEqual(hexString, intelHexFile2.ToString());

        }
    }
}
