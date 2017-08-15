namespace BlockchainParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class MultipleFilesStream : Stream
    {
        private readonly Stream[] _streams;
        private readonly String[] _paths;
        private readonly long[] _streamsLength;

        private Stream _currentStream;
        private int _currentStreamIndex;
        private long _length = -1;
        private long _position;

        public MultipleFilesStream(IEnumerable<KeyValuePair<string, Stream>> streams)
        {
            if (streams == null)
                throw new ArgumentNullException(nameof(streams));

            _streams = streams.Select(t => t.Value).ToArray();
            _paths = streams.Select(t => t.Key).ToArray();
            _currentStreamIndex = 0;
            _currentStream = _streams[_currentStreamIndex];
            _streamsLength = _streams.Select(x => x.Length).ToArray();
        }

        public Tuple<string, long, long> GetCurrentStreamMetadata()
        {
            return new Tuple<string, long, long>(_paths[_currentStreamIndex], _streams[_currentStreamIndex].Position, _position);
        }

        public override void Flush()
        {
            if (_currentStream != null)
                _currentStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long pos = 0;
            int streamIndex = 0;

            if (origin == SeekOrigin.Begin)
            {
                while (pos + _streamsLength[streamIndex] <= offset)
                {
                    pos += _streamsLength[streamIndex];
                    streamIndex++;
                }

                if (streamIndex != _currentStreamIndex)
                {
                    _currentStreamIndex = streamIndex;
                    _currentStream = _streams[_currentStreamIndex];
                }
                _currentStream.Seek(offset - pos, origin);

                _position = offset;
                return offset;
            }
            else
            {
                while (pos + _streamsLength[streamIndex] <= offset + _position)
                {
                    pos += _streamsLength[streamIndex];
                    streamIndex++;
                }

                if (streamIndex != _currentStreamIndex)
                {
                    _currentStreamIndex = streamIndex;
                    _currentStream = _streams[_currentStreamIndex];
                }
                _currentStream.Seek(offset, origin);

                _position += offset;
                return offset;
            }
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException("It is not possible to set the Stream length.");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;
            int buffPostion = offset;

            while (count > 0)
            {
                int bytesRead = _currentStream.Read(buffer, buffPostion, count);
                result += bytesRead;
                buffPostion += bytesRead;
                _position += bytesRead;

                if (bytesRead <= count)
                {
                    count -= bytesRead;
                }

                if (count > 0)
                {
                    if (_currentStreamIndex >= _streams.Length - 1)
                    {
                        break;
                    }

                    _currentStream = _streams[++_currentStreamIndex];
                }
            }

            return result;
        }

        public override long Length
        {
            get
            {
                if (_length == -1)
                {
                    _length = 0;
                    foreach (var stream in _streams)
                    {
                        _length += stream.Length;
                    }
                }

                return _length;
            }
        }

        public override long Position
        {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Stream is not writable");
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    for (var i = _streams.Length - 1; i >= 0; i--)
                    {
                        var stream = _streams[i];
                        stream.Dispose();
                        _streams[i] = null;
                        _paths[i] = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
