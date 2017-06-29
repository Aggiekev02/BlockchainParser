using System;
using System.Collections.Generic;
using System.IO;

namespace Temosoft.Bitcoin.Blockchain
{
    public class Block
    {
        private static DateTime _epochBaseDate = new DateTime(1970,1,1);
        private readonly long _position;
        private Stream _stream;
        private readonly Lazy<BinaryReader> _reader;

        public uint HeaderLength;

        private uint _versionNumber;
        private byte[] _previousBlockHash;
        private byte[] _merkleRoot;
        private DateTime _timeStamp;
        private uint _lockTime;
        private long _size;
        private uint _nonce;
        private uint _bits;
        private long _transactionCount;

        public uint VersionNumber
        {
            get
            {
                ReadHeader();
                return _versionNumber;
            }
        }

        public byte[] PreviousBlockHash
        {
            get
            {
                ReadHeader();
                return _previousBlockHash;
            }
        }

        public byte[] MerkleRoot
        {
            get
            {
                ReadHeader();
                return _merkleRoot;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                ReadHeader();
                return _timeStamp;
            }
        }
        public uint Bits
        {
            get
            {
                ReadHeader();
                return _bits;
            }
        }
        public uint Nonce
        {
            get
            {
                ReadHeader();
                return _nonce;
            }
        }
        public uint LockTime
        {
            get
            {
                ReadHeader();
                return _lockTime;
            }
        }
        public long Size
        {
            get
            {
                ReadHeader();
                return _size;
            }
        }

        private void ReadHeader()
        {
            if(_versionNumber > 0) return;
            var r = _reader.Value;
            r.BaseStream.Position = _position + 4;
            _versionNumber = r.ReadUInt32();
            _previousBlockHash = r.ReadHashAsByteArray();
            _merkleRoot = r.ReadHashAsByteArray();
            _timeStamp = _epochBaseDate.AddSeconds(r.ReadUInt32());
            _bits = r.ReadUInt32();
            _nonce = r.ReadUInt32();
            _transactionCount = r.ReadVarInt();
        }

        public double Difficulty
        {
            get { return CalculateDifficulty(); }
        }

        public IEnumerable<Transaction> Transactions
        {
            get
            {
                var r = _reader.Value;
                for (var ti = 0; ti < _transactionCount; ti++)
                {
                    var t = new Transaction();
                    t.VersionNumber = r.ReadInt32();

                    var inputCount = r.ReadVarInt();
                    if (inputCount == 0)
                        t.Inputs = null;
                    else
                        t.Inputs = ParseInputs(inputCount, r);

                    var outputCount = r.ReadVarInt();
                    if (outputCount == 0)
                        t.Outputs = null;
                    else
                        t.Outputs = ParseOutputs(outputCount, r);

                    t.LockTime = r.ReadUInt32();

                    yield return t;
                }
            }
        }

        private static Input[] ParseInputs(long inputCount, BinaryReader r)
        {
            var inputs = new List<Input>((int)inputCount);

            for (var i = 0; i < inputCount; i++)
            {
                var input = new Input
                {
                    TransactionHash = r.ReadBytes(32),
                    TransactionIndex = r.ReadUInt32()
                };

                var scriptLength = r.ReadVarInt();
                input.Script = r.ReadBytes((int)scriptLength);

                input.SequenceNumber = r.ReadUInt32();

                inputs.Add(input);
            }

            return inputs.ToArray();
        }

        private static Output[] ParseOutputs(long outputCount, BinaryReader r)
        {
            var outputs = new List<Output>((int)outputCount);

            for (var i = 0; i < outputCount; i++)
            {
                var output = new Output();

                output.Value = r.ReadUInt64();

                var length = r.ReadVarInt();

                output.Script = r.ReadBytes((int)length);

                outputs.Add(output);
            }

            return outputs.ToArray();
        }

        public Block(Stream stream)
        {
            _position = stream.Position;
            _stream = stream;
            _reader = new Lazy<BinaryReader>(() => new BinaryReader(stream));
        }

        private double CalculateDifficulty()
        {
            return CalculateDifficulty(Bits);
        }

        //static double max_body = fast_log(0x00ffff), scaland = fast_log(256);
        private static readonly double max_body = Math.Log(0x00ffff);
        private static readonly double scaland = Math.Log(256);

        private static double CalculateDifficulty(uint bits)
        {
            //return exp(max_body - fast_log(bits & 0x00ffffff) + scaland * (0x1d - ((bits & 0xff000000) >> 24)));
            var part1 = Math.Log(bits & 0x00ffffff);
            var part2 = 0x1d - ((bits & 0xff000000) >> 24);
            var exp = Math.Exp(max_body - part1 + scaland * part2);

            return exp;
        }
    }
}