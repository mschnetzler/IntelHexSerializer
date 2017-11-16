using IntelHexSerializer.Exceptions;
using IntelHexSerializer.File;
using IntelHexSerializer.File.Record;
using NUnit.Framework;

namespace IntelHexSerializerTest.File
{
    [TestFixture]
    public class OffsetRecordTest 
    {
   
        [TestCase(new byte[] { 0x00, 0x01 }, 0xF9, 0x010000, 0x00000000)]
        [TestCase(new byte[] { 0xFF, 0xFF }, 0xFC, 0x010000, 0xFFFe0000)]
        [TestCase(new byte[] { 0x12, 0x34 }, 0xB4, 0x010000, 0x12330000)]
        public void OffsetRecordShouldFillUpTheOffset(byte[] data, byte checksum, long byteLength, long baseAddress)
        {
            var expectedBinaryRepresentation = new byte[byteLength];

            for (var i = 0; i < byteLength; i++)
                expectedBinaryRepresentation[i] = 0xFF;

            var sut = new OffsetRecord()
            {
                Address = 0x0000,
                ByteCount = 2,
                Data = data,
                Checksum = checksum,
                Type = IntelHexRecordType.ExtendedLinearAddress
            };

            var caluclatedBinaryRepresentation = sut.GetBinaryRepresentation((int)baseAddress);
            Assert.AreEqual(expectedBinaryRepresentation, caluclatedBinaryRepresentation);
        }

        [TestCase(new byte[] { 0x01, 0x00 }, 0xAA)]
        [TestCase(new byte[] { 0xFF, 0xFF }, 0x16)]
        [TestCase(new byte[] { 0x34, 0x12 }, 0x99)]
        public void ChecksumValidationExceptionShouldBeThrownIfChecksumIsWrong(byte[] data, byte checksum)
        {
            var sut = new OffsetRecord()
            {
                Address = 0x0000,
                ByteCount = 2,
                Data = data,
                Checksum = checksum,
                Type = IntelHexRecordType.ExtendedLinearAddress
            };

            Assert.Throws<ChecksumValidationFailedException>(() => sut.GetBinaryRepresentation(0x0000));

        }
    }
}
