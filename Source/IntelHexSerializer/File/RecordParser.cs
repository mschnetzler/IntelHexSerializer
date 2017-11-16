#region Usings

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using IntelHexSerializer.File.Record;

#endregion

namespace IntelHexSerializer.File
{
    public static class RecordParser
    {
        private const string Pattern =
            "(?<Start>:)(?<ByteCount>.{2})(?<Address>.{4})(?<RecordType>.{2})(?<Data>.*)(?<Checksum>.{2})";

        public static IntelHexRecord ParseRecord(string hexRecordString)
        {
            var regex = new Regex(Pattern);
            var match = regex.Match(hexRecordString);

            var byteCount = byte.Parse(match.Groups["ByteCount"].Value, NumberStyles.HexNumber);
            var address = ushort.Parse(match.Groups["Address"].Value, NumberStyles.HexNumber);
            var recordType =
                (IntelHexRecordType) Enum.Parse(typeof(IntelHexRecordType), match.Groups["RecordType"].Value);
            var stringData = match.Groups["Data"].Value;
            var checksum = byte.Parse(match.Groups["Checksum"].Value, NumberStyles.HexNumber);

            var data = new byte[stringData.Length / 2];

            for (var i = 0; i < data.Length; i++)
                data[i] = byte.Parse(stringData.Substring(i * 2, 2), NumberStyles.HexNumber);

            var record = GetRecord(recordType);

            record.Data = data;
            record.Address = address;
            record.ByteCount = byteCount;
            record.Checksum = checksum;
            record.Type = recordType;

            return record;
        }

        private static IntelHexRecord GetRecord(IntelHexRecordType recordType)
        {
            switch (recordType)
            {
                case IntelHexRecordType.Data:
                    return new DataRecord();
                case IntelHexRecordType.ExtendedLinearAddress:
                    return new OffsetRecord();
                case IntelHexRecordType.StartLinearAddress:
                    return new StartAddressRecord();
                case IntelHexRecordType.EndOfFile:
                    return new EndOfFileRecord();
                default:
                    throw new NotImplementedException($"Recordtype {recordType.ToString()} is not implemented");
            }
        }
    }
}