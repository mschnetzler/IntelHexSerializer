using IntelHexSerializer.File;
using NUnit.Framework;

namespace IntelHexSerializerTest.File
{
    [TestFixture]
    public class DataRecordTest
    {
        [TestCase(":10000000E0A5E6F6FDFFE0AEE00FE6FCFDFFE6FD55", 0x00000000, 
            new byte[]
            {
                0xE0, 0xA5, 0xE6, 0xF6, 0xFD, 0xFF, 0xE0, 0xAE,
                0xE0, 0x0F, 0xE6, 0xFC, 0xFD, 0xFF, 0xE6, 0xFD
            })]
        [TestCase(":100010003F0156702B5E712B722B732146013421E8", 0x00000000,
            new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0x3F, 0x01, 0x56, 0x70, 0x2B, 0x5E, 0x71, 0x2B,
                0x72, 0x2B, 0x73, 0x21, 0x46, 0x01, 0x34, 0x21
            })]
        [TestCase(":100010003F0156702B5E712B722B732146013421E8", 0x00000010,
            new byte[]
            {
                0x3F, 0x01, 0x56, 0x70, 0x2B, 0x5E, 0x71, 0x2B,
                0x72, 0x2B, 0x73, 0x21, 0x46, 0x01, 0x34, 0x21
            })]
        public void TestGetBinaryRepresentation(string hexDataRecord, int baseAddress, byte[] expectedBinaryRepresentation)
        {
            var uut = RecordParser.ParseRecord(hexDataRecord);

            var representation = uut.GetBinaryRepresentation(baseAddress);
            Assert.AreEqual(expectedBinaryRepresentation, representation);
        }
    }
}
