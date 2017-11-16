namespace IntelHexSerializer.Util
{
    public class ByteArray
    {
        public static byte[] Create(int length, byte defaultValue = 0)
        {
            var array = new byte[length];
            for (var index = 0; index < length; index++)
                array[index] = defaultValue;

            return array;

        }
     }
}
