namespace IntelHexSerializer.File.Record
{
    public abstract class IntelHexRecord
    {
        #region Properties

        public int ByteCount { get; set; }

        public virtual int Address { get; set; }

        public IntelHexRecordType Type { get; set; }

        public byte[] Data { get; set; }

        public byte Checksum { get; set; }

        #endregion

        /// <summary>
        /// Returns the binary representation of this single IntelHexRecord
        /// </summary>
        /// <returns>the binary representation o</returns>
        public abstract byte[] GetBinaryRepresentation(int fromAddress);

        public abstract int GetOffsetAddress();

        public abstract int GetEndAddress();

        public abstract void AddAddress(int address);

        /// <summary>
        /// Calculates the checksum of the 
        /// </summary>
        /// <returns></returns>
        public byte CalculateChecksum()
        {
            var checksum = ByteCount +
                           (Address & 0xFF) +
                           ((Address >> 8) & 0xFF) +
                           (byte) Type;

            foreach (var singleByte in Data)
                checksum += singleByte;

            return (byte) (~checksum + 1);
        }

        public override string ToString()
        {
            var dataString = string.Empty;
            foreach (var dataByte in Data)
                dataString = $"{dataString}{dataByte:x2}";

            var firstAddressByte = (Address & 0xFF);
            var secondAddressByte = ((Address >> 8) & 0xFF);

            return
                $":{ByteCount:x2}{secondAddressByte:x2}{firstAddressByte:x2}{((byte) Type):x2}{dataString}{Checksum:x2}"
                    .ToUpper();
        }
    }
}