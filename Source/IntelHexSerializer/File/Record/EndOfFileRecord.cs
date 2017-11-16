namespace IntelHexSerializer.File.Record
{
    public class EndOfFileRecord : IntelHexRecord
    {
        //:00000001FF
        public EndOfFileRecord()
        {
            Address = 0x0000;
            ByteCount = 0;
            Data = new byte[0];
            Checksum = 0xFF;
            Type = IntelHexRecordType.EndOfFile;
        }

        public override byte[] GetBinaryRepresentation(int fromAddress)
        {
            return new byte[] { };
        }

        public override int GetOffsetAddress()
        {
            return 0;
        }

        public override int GetEndAddress()
        {
            return 0;
        }

        public override void AddAddress(int address)
        {
        }
    }
}