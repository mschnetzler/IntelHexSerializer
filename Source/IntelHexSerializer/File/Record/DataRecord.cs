#region Usings

using System.Linq;
using IntelHexSerializer.Exceptions;

#endregion

namespace IntelHexSerializer.File.Record
{
    public class DataRecord : IntelHexRecord
    {
        public DataRecord()
        {
        }

        public DataRecord(int address, byte[] data)
        {
            Address = address;
            ByteCount = data.Length;
            Data = data;
            Type = IntelHexRecordType.Data;
            Checksum = CalculateChecksum();
        }

        public override byte[] GetBinaryRepresentation(int fromAddress)
        {
            var calculatedChecksum = CalculateChecksum();

            if (calculatedChecksum != Checksum)
                throw new ChecksumValidationFailedException(calculatedChecksum, Checksum);

            var offset = Address - fromAddress;
            var offsetArray = new byte[offset];
            for (var index = 0; index < offset; index++)
                offsetArray[index] = 0xFF;

            return offsetArray.Concat(Data).ToArray();
        }

        public override int GetOffsetAddress()
        {
            return 0;
        }

        public override int GetEndAddress()
        {
            return Address + ByteCount;
        }

        public override void AddAddress(int address)
        {
            Address += address;
            Checksum = CalculateChecksum();
        }

        public void TrimEnd()
        {
            var dataList = Data.ToList();

            while (dataList.Any() && dataList.Last() == 0xFF)
                dataList.RemoveAt(dataList.Count - 1);

            Data = dataList.ToArray();
            ByteCount = Data.Length;
            Checksum = CalculateChecksum();
        }
    }
}