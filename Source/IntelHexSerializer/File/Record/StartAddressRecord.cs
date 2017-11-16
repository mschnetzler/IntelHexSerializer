namespace IntelHexSerializer.File.Record
{
    public class StartAddressRecord : IntelHexRecord
    {
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