namespace IntelHexSerializer.File
{
    public class OffsetSeekResult
    {
        #region Properties

        public int FromIndex { get; }
        public int ToIndex { get; }

        public bool Match => FromIndex < ToIndex;

        #endregion

        public OffsetSeekResult(int fromIndex, int toIndex)
        {
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }
}