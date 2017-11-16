using IntelHexSerializer.File;
using IntelHexSerializer.File.Record;
using NUnit.Framework;

namespace IntelHexSerializerTest.File
{
    public class RecordParserTest
    {
        [TestCase(":020000040804EE", 2, 0, IntelHexRecordType.ExtendedLinearAddress, new byte[] { 0x08, 0x04}, 0xEE)]
        [TestCase(":100000000000022071020408750204087702040847", 16, 0, IntelHexRecordType.Data,
            new byte[] { 0x00, 0x00, 0x02, 0x20, 0x71, 0x02, 0x04, 0x08,
                         0x75, 0x02, 0x04, 0x08, 0x77, 0x02, 0x04, 0x08 },
            0x47)]

        [TestCase(":10028000EFF3098071461A4A104710B5084C236BEA", 16, 0x0280, IntelHexRecordType.Data,
            new byte[] { 0xEF, 0xF3, 0x09, 0x80, 0x71, 0x46, 0x1A, 0x4A,
                         0x10, 0x47, 0x10, 0xB5, 0x08, 0x4C, 0x23, 0x6B },
            0xEA)]
        public void ParseRecordTest(string record, 
            int byteCount, 
            int address, 
            IntelHexRecordType recordType,
            byte[] data, 
            byte checksum)
        {
            var intelHexRecord = RecordParser.ParseRecord(record);

            Assert.AreEqual(byteCount, intelHexRecord.ByteCount);
            Assert.AreEqual(address, intelHexRecord.Address);
            Assert.AreEqual(recordType, intelHexRecord.Type);
            Assert.AreEqual(data, intelHexRecord.Data);
            Assert.AreEqual(checksum, intelHexRecord.Checksum);
        }
    }
}
