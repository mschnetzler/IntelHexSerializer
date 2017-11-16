using IntelHexSerializer.File;
using NUnit.Framework;

namespace IntelHexSerializerTest.File
{
    [TestFixture]
    public class CalculateChecksumTest
    {
        [TestCase(":10010000FD020408FD020408FD020408FD020408C3", 0xC3)]
        [TestCase(":020000040804EE", 0xEE)]
        [TestCase(":0C0DC0000C000020000000000024F400E3", 0xE3)]
        public void CalculateChecksumShouldCalculateTheCorrectChecksum(string record, byte checksum)
        {
            var sut = RecordParser.ParseRecord(record);
            Assert.AreEqual(checksum, sut.CalculateChecksum());
        }
    }
}
