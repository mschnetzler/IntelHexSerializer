#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IntelHexSerializer.File.Record;
using IntelHexSerializer.Util;

#endregion

namespace IntelHexSerializer.File
{
    public class IntelHexFile
    {
        #region Properties

        public int? StartAddress => _records.OfType<DataRecord>().FirstOrDefault()?.Address;

        public int? EndAddress => _records.OfType<DataRecord>().LastOrDefault()?.GetEndAddress();

        /// <summary>
        /// The binary data Representation of the hex file
        /// !Attention! this does change the values of StartAddress, EndAddressand ToString()
        /// </summary>
        public byte[] BinaryData => CalculateBinaryArray();

        #endregion

        private readonly int _baseAddress;

        private readonly List<IntelHexRecord> _records = new List<IntelHexRecord>();

        private IntelHexFile(int baseAddress)
        {
            _baseAddress = baseAddress;
        }

        /// <summary>
        /// Returns the string representation of this IntelHexFile
        /// </summary>
        /// <returns>A valid intel hexfile string</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var first = true;
            foreach (var record in _records)
            {
                if (!first)
                    stringBuilder.AppendLine();

                stringBuilder.Append(record);
                first = false;
            }

            return stringBuilder.ToString();
        }


        public byte[] CalculateBinaryArray()
        {
            var lastAddress = _baseAddress;
            var array = ByteArray.Create(EndAddress.Value - lastAddress, 0xFF);
            var pointer = 0;

            foreach (var record in _records)
            {
                var recordArray = record.GetBinaryRepresentation(lastAddress);

                Buffer.BlockCopy(recordArray, 0, array, pointer, recordArray.Length);

                pointer += recordArray.Length;
                lastAddress = record.GetEndAddress();
            }

            return array;
        }

        /// <summary>
        /// Creates a new IntelHexFile out of an Intel Hexfile string
        /// </summary>
        /// <param name="intelHexString">The string representation of an intel hexfile</param>
        /// <returns>An new IntelHexFile representation</returns>
        public static IntelHexFile CreateFrom(string intelHexString, int baseAddress = 0x00000000)
        {
            var hexFile = new IntelHexFile(baseAddress);
            var reader = new StringReader(intelHexString);

            string line;
            while ((line = reader.ReadLine()) != null)
                hexFile._records.Add(RecordParser.ParseRecord(line));

            //Calculate addresses
            var currentOffsetAddress = 0x00000000;
            foreach (var record in hexFile._records)
            {
                if (record.GetOffsetAddress() > currentOffsetAddress)
                {
                    currentOffsetAddress = record.GetOffsetAddress();
                    continue;
                }

                record.Address += currentOffsetAddress;
            }

            return hexFile;
        }

        private static short CalculateOffset(IntelHexRecord first, IntelHexRecord second)
        {
            var offset = second.Address - (first.Address + first.ByteCount);

            if (offset > short.MaxValue)
                throw new NotImplementedException("an offset higher than 2byte cannot be handeled");

            return (short) offset;
        }

        /// <summary>
        /// Create a new IntelHexFile out of the binary data
        /// </summary>
        /// <param name="binaryData">The binary source code representation</param>
        /// <returns>An new IntelHexFile representation</returns>
        public static IntelHexFile CreateFrom(byte[] binaryData, int baseAddress = 0x00000000)
        {
            var hexFile = new IntelHexFile(baseAddress);

            hexFile._records.AddRange(CreateDataRecords(binaryData));
            hexFile._records.ForEach(record => record.AddAddress(baseAddress));

            var seekResult = SeekForOffset(hexFile);
            while (seekResult.Match)
            {
                var fromRecord = hexFile._records[seekResult.FromIndex];
                var toRecord = hexFile._records[seekResult.ToIndex];

                hexFile._records.RemoveRange(seekResult.FromIndex, seekResult.ToIndex - seekResult.FromIndex);

                var addressDelta = toRecord.Address;
                if (addressDelta > short.MaxValue)
                {
                    var highOffsetPart = (short) ((toRecord.Address >> 16) & 0xFFFF);
                    hexFile._records.Insert(seekResult.FromIndex, new OffsetRecord(highOffsetPart, 0));
                }

                seekResult = SeekForOffset(hexFile);
            }

            foreach (var dataRecord in hexFile._records.OfType<DataRecord>())
                dataRecord.TrimEnd();

            var insertCount = 0;
            for (var index = 0; index + insertCount + 1 < hexFile._records.Count; index++)
            {
                IntelHexRecord previousRecord = null;
                if(index != 0)
                    previousRecord = hexFile._records[index + insertCount -1];

                var nextRecord = hexFile._records[index + insertCount];

                if(nextRecord is OffsetRecord || previousRecord is OffsetRecord)
                    continue;

                var previousOffsetAddressPart = (short)((previousRecord?.Address ?? 0) >> 16);
                var nextOffsetAddressPart = (short)(nextRecord.Address >> 16);

                if (previousOffsetAddressPart == nextOffsetAddressPart) continue;
                hexFile._records.Insert(index + insertCount, new OffsetRecord(nextOffsetAddressPart, 0));
                insertCount++;
            }

            return hexFile;
        }

        private static OffsetSeekResult SeekForOffset(IntelHexFile hexFile)
        {
            var records = hexFile._records.ToArray();

            var offsetStart = 0;

            for (var index = 0; index < records.Length; index++)
            {
                var record = records[index];
                if (RecordIsOffset(record)) continue;

                if (index == offsetStart)
                {
                    offsetStart++;
                    continue;
                }

                return new OffsetSeekResult(offsetStart, index);
            }

            return new OffsetSeekResult(offsetStart, offsetStart);
        }

        private static bool RecordIsOffset(IntelHexRecord record)
        {
            return record.Data.All(b => b == 0xFF);
        }


        private static IEnumerable<IntelHexRecord> CreateDataRecords(byte[] binaryData)
        {
            var records = new List<IntelHexRecord>();
            var blockSize = 0x10;
            var blockCount = binaryData.Length / blockSize + 1;

            for (var index = 0; index < blockCount; index++)
            {
                var remainingBytes = binaryData.Length - (index * blockSize);
                if (remainingBytes == 0) break;

                var take = Math.Min(blockSize, remainingBytes);
                var block = ByteArray.Create(blockSize, 0xFF);

                Buffer.BlockCopy(binaryData, index * blockSize, block, 0, take);
                var address = index * blockSize;
                records.Add(new DataRecord(address, block));
            }

            records.Add(new EndOfFileRecord());

            return records;
        }
    }
}