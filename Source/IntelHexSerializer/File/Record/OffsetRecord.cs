#region Usings

using System;
using System.Linq;
using IntelHexSerializer.Exceptions;

#endregion

namespace IntelHexSerializer.File.Record
{
    public class OffsetRecord : IntelHexRecord
    {
        #region Properties

        public override int Address
        {
            get { return 0x0000; }
            set { _address = value; }
        }

        #endregion

        private int _address;

        public OffsetRecord()
        {
        }

        public OffsetRecord(short offset, int address)
        {
            Address = address;
            ByteCount = 2;
            Type = IntelHexRecordType.ExtendedLinearAddress;

            var firstDataByte = (byte) (offset & 0xFF);
            var secondDataByte = (byte) ((offset >> 8) & 0xFF);
            Data = new[] {secondDataByte, firstDataByte};

            Checksum = CalculateChecksum();
        }

        public override byte[] GetBinaryRepresentation(int fromAddress)
        {
            if (Data.Length > 8)
                throw new Exception("linear address cannot be longer than 32bit");

            var calculateChecksum = CalculateChecksum();
            if (Checksum != calculateChecksum)
                throw new ChecksumValidationFailedException(calculateChecksum, Checksum);

            var shortOffset = GetAddressOffset();
            var offsetDataLength = ((int) (shortOffset << 16)) - fromAddress;

            if (offsetDataLength < 0)
                throw new NotImplementedException("Baseaddress cannot be higher than the offset");

            var offsetData = new byte[offsetDataLength];

            for (var i = 0; i < offsetDataLength; i++)
                offsetData[i] = 0xFF;

            return offsetData;
        }

        public override int GetOffsetAddress()
        {
            return ((int) (GetAddressOffset() << 16));
        }

        public override int GetEndAddress()
        {
            return GetOffsetAddress();
        }

        public override void AddAddress(int address)
        {
            var offsetAddress = ((int) (GetAddressOffset() << 16));
            offsetAddress += address;
            var shortOffset = (short) (offsetAddress >> 16);
            SetAddressOffset(shortOffset);
            Checksum = CalculateChecksum();
        }


        public short GetAddressOffset()
        {
            var data = Data.Reverse().ToArray();
            return BitConverter.ToInt16(data, 0);
        }

        public void SetAddressOffset(short offset)
        {
            var data = BitConverter.GetBytes(offset);
            Data = data.Reverse().ToArray();
        }
    }
}