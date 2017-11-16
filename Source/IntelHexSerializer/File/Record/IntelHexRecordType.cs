namespace IntelHexSerializer.File.Record
{
    public enum IntelHexRecordType
    {
        Data = 0,
        EndOfFile = 1,
        ExtendedSegmentAddresss = 2,
        StartSegmentAddress = 3,
        ExtendedLinearAddress = 4,
        StartLinearAddress = 5
    }
}