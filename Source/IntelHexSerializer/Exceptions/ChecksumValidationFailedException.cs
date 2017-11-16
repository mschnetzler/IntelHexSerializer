#region Usings

using System;

#endregion

namespace IntelHexSerializer.Exceptions
{
    public class ChecksumValidationFailedException : Exception
    {
        public ChecksumValidationFailedException(byte expcted, byte read) : base(
            $"expected checksum: {expcted:x2}, but was {read:x2}")
        {
        }
    }
}