using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace System.IO
{
    [Serializable]
    public class ArrayPoolMemoryStream : Stream
    {
        private ArrayPool<byte> _arrayPool;
        private byte[] _buffer; // Either allocated internally or externally.
        private int _origin; // For user-provided arrays, start at this origin
        private int _position; // read/write head.
        [ContractPublicPropertyName("Length")] private int _length; // Number of bytes within the memory stream
        private int _capacity; // length of usable portion of buffer for stream
        // Note that _capacity == _buffer.Length for non-user-provided byte[]'s

        private bool _expandable; // User-provided buffers aren't expandable.
        private bool _writable; // Can user write to this stream?
        private bool _exposable; // Whether the array can be returned to the user.
        private bool _isOpen; // Is this stream open or closed?
        private static readonly byte[] EmptyByteArray = new byte[0];
        private const int MaxByteArrayLength = 0x7FFFFFC7;

#if FEATURE_ASYNC_IO
        [NonSerialized]
        private Task<int> _lastReadTask; // The last successful task returned from ReadAsync
#endif

        private const int MemStreamMaxLength = Int32.MaxValue;

        public ArrayPoolMemoryStream(ArrayPool<byte> arrayPool = null) 
            : this(0, arrayPool) {
        }

        public ArrayPoolMemoryStream(int capacity, ArrayPool<byte> arrayPool = null)
        {
            _arrayPool = arrayPool ?? ArrayPool<byte>.Shared;
            _buffer = RequestBuffer(capacity);
            _capacity = capacity;
            _expandable = true;
            _writable = true;
            _exposable = false; // true;
            _origin = 0; // Must be 0 for byte[]'s created by MemoryStream
            _isOpen = true;
        }

        protected virtual byte[] RequestBuffer(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative");
            if (capacity == 0)
                return EmptyByteArray;
            return _arrayPool.Rent(capacity);
        }

        protected virtual void ReleaseBuffer(byte[] buffer)
        {
            if (buffer.Length == 0)
                return;
            _arrayPool.Return(buffer, true);
        }

        public override bool CanRead
        {
            [Pure] get { return _isOpen; }
        }

        public override bool CanSeek
        {
            [Pure] get { return _isOpen; }
        }

        public override bool CanWrite
        {
            [Pure] get { return _writable; }
        }

        private void EnsureWriteable()
        {
            if (!CanWrite) throw new InvalidOperationException("Write not supported");
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _isOpen = false;
                    _writable = false;
                    _expandable = false;
                    ReleaseBuffer(_buffer);
                    // This requirement do not hold for managed pool's buffers: Don't set buffer to null - allow TryGetBuffer, GetBuffer & ToArray to work.
#if FEATURE_ASYNC_IO
                    _lastReadTask = null;
#endif
                }
            }
            finally
            {
                // Call base.Close() to cleanup async IO resources
                base.Dispose(disposing);
            }
        }

        // returns a bool saying whether we allocated a new array.
        private bool EnsureCapacity(int value)
        {
            // Check for overflow
            if (value < 0)
                throw new IOException("Stream too long");
            if (value > _capacity)
            {
                int newCapacity = value;
                if (newCapacity < 256)
                    newCapacity = 256;
                // We are ok with this overflowing since the next statement will deal
                // with the cases where _capacity*2 overflows.
                if (newCapacity < _capacity*2)
                    newCapacity = _capacity*2;
                // We want to expand the array up to Array.MaxArrayLengthOneDimensional
                // And we want to give the user the value that they asked for
                if ((uint) (_capacity*2) > MaxByteArrayLength)
                    newCapacity = value > MaxByteArrayLength ? value : MaxByteArrayLength;

                Capacity = newCapacity;
                return true;
            }
            return false;
        }

        public override void Flush()
        {
        }

#if FEATURE_ASYNC_IO
        [HostProtection(ExternalThreading=true)]
        [ComVisible(false)]
        public override Task FlushAsync(CancellationToken cancellationToken) {

            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            try {

                Flush();
                return Task.CompletedTask;
        
            } catch(Exception ex) {

                return Task.FromException(ex);
            }
        }
#endif // FEATURE_ASYNC_IO


        public virtual byte[] GetBuffer()
        {
            if (!_exposable)
                throw new UnauthorizedAccessException("Unauthorized access to stream buffer");
            return _buffer;
        }

        public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (!_exposable)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }

            buffer = new ArraySegment<byte>(_buffer, offset: _origin, count: (_length - _origin));
            return true;
        }

        // PERF: True cursor position, we don't need _origin for direct access
        internal int InternalGetPosition()
        {
            if (!_isOpen) throw new IOException("Stream is closed");
            return _position;
        }

        // PERF: Takes out Int32 as fast as possible
        internal int InternalReadInt32()
        {
            if (!_isOpen)
                throw new IOException("Stream is closed");

            int pos = (_position += 4); // use temp to avoid ----
            if (pos > _length)
            {
                _position = _length;
                throw new IOException("End of file");
            }
            return (int) (_buffer[pos - 4] | _buffer[pos - 3] << 8 | _buffer[pos - 2] << 16 | _buffer[pos - 1] << 24);
        }

        // PERF: Get actual length of bytes available for read; do sanity checks; shift position - i.e. everything except actual copying bytes
        internal int InternalEmulateRead(int count)
        {
            if (!_isOpen) throw new IOException("Stream is closed");

            int n = _length - _position;
            if (n > count) n = count;
            if (n < 0) n = 0;

            Contract.Assert(_position + n >= 0, "_position + n >= 0"); // len is less than 2^31 -1.
            _position += n;
            return n;
        }

        // Gets & sets the capacity (number of bytes allocated) for this stream.
        // The capacity cannot be set to a value less than the current length
        // of the stream.
        // 
        public virtual int Capacity
        {
            get
            {
                if (!_isOpen) throw new IOException("Stream is closed");
                return _capacity - _origin;
            }
            set
            {
                // Only update the capacity if the MS is expandable and the value is different than the current capacity.
                // Special behavior if the MS isn't expandable: we don't throw if value is the same as the current capacity
                if (value < Length)
                    throw new ArgumentOutOfRangeException(nameof(value), "Small capacity");
                Contract.Ensures(_capacity - _origin == value);
                Contract.EndContractBlock();

                if (!_isOpen) throw new IOException("Stream is closed"); 
                if (!_expandable && (value != Capacity)) throw new InvalidOperationException("Memory stream is not expandable");

                // MemoryStream has this invariant: _origin > 0 => !expandable (see ctors)
                if (_expandable && value != _capacity)
                {
                    if (value > 0)
                    {
                        byte[] newBuffer = RequestBuffer(value); // new byte[value];
                        byte[] oldBuffer = _buffer;
                        if (_length > 0) Buffer.BlockCopy(oldBuffer, 0, newBuffer, 0, _length);
                        _buffer = newBuffer;
                        ReleaseBuffer(oldBuffer);
                    }
                    else
                    {
                        _buffer = null;
                    }
                    _capacity = value;
                }
            }
        }

        public override long Length
        {
            get
            {
                if (!_isOpen) throw new IOException("Stream is closed");
                return _length - _origin;
            }
        }

        public override long Position
        {
            get
            {
                if (!_isOpen) throw new IOException("Stream is closed");
                return _position - _origin;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Need non-negative number");
                Contract.Ensures(Position == value);
                Contract.EndContractBlock();

                if (!_isOpen) throw new IOException("Stream is closed");

                if (value > MemStreamMaxLength)
                    throw new ArgumentOutOfRangeException(nameof(value), "Stream length");
                _position = _origin + (int) value;
            }
        }

        public override int Read([In, Out] byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset and length");
            Contract.EndContractBlock();

            if (!_isOpen) throw new IOException("Stream is closed");

            int n = _length - _position;
            if (n > count) n = count;
            if (n <= 0)
                return 0;

            Contract.Assert(_position + n >= 0, "_position + n >= 0"); // len is less than 2^31 -1.

            if (n <= 8)
            {
                int byteCount = n;
                while (--byteCount >= 0)
                    buffer[offset + byteCount] = _buffer[_position + byteCount];
            }
            else
                Buffer.BlockCopy(_buffer, _position, buffer, offset, n);
            _position += n;

            return n;
        }

#if FEATURE_ASYNC_IO
        [HostProtection(ExternalThreading = true)]
        [ComVisible(false)]
        public override Task<int> ReadAsync(Byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (buffer==null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset and length");
            Contract.EndContractBlock(); // contract validation copied from Read(...)

            // If cancellation was requested, bail early
            if (cancellationToken.IsCancellationRequested) 
                return Task.FromCanceled<int>(cancellationToken);

            try
            {
                int n = Read(buffer, offset, count);
                var t = _lastReadTask;
                Contract.Assert(t == null || t.Status == TaskStatus.RanToCompletion, 
                    "Expected that a stored last task completed successfully");
                return (t != null && t.Result == n) ? t : (_lastReadTask = Task.FromResult<int>(n));
            }
            catch (OperationCanceledException oce)
            {
                return Task.FromCanceled<int>(cancellationToken);
            }
            catch (Exception exception)
            {
                return Task.FromException<int>(exception);
            }
        }
#endif //FEATURE_ASYNC_IO


        public override int ReadByte()
        {
            if (!_isOpen) throw new IOException("Stream is closed");

            if (_position >= _length) return -1;

            return _buffer[_position++];
        }


#if FEATURE_ASYNC_IO
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) {

            // This implementation offers beter performance compared to the base class version.

            // The parameter checks must be in [....] with the base version:
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            if (!CanRead && !CanWrite)
                throw new ObjectDisposedException(null, "Stream is closed");

            if (!destination.CanRead && !destination.CanWrite)
                throw new ObjectDisposedException(nameof(destination), "Stream is closed");

            if (!CanRead)
                throw new NotSupportedException("Unreadable stream");

            if (!destination.CanWrite)
                throw new NotSupportedException("Unwriteable stream");

            Contract.EndContractBlock();

            // If we have been inherited into a subclass, the following implementation could be incorrect
            // since it does not call through to Read() or Write() which a subclass might have overriden.  
            // To be safe we will only use this implementation in cases where we know it is safe to do so,
            // and delegate to our base class (which will call into Read/Write) when we are not sure.
            if (this.GetType() != typeof(MemoryStream))
                return base.CopyToAsync(destination, bufferSize, cancellationToken);

            // If cancelled - return fast:
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
           
            // Avoid copying data from this buffer into a temp buffer:
            //   (require that InternalEmulateRead does not throw,
            //    otherwise it needs to be wrapped into try-catch-Task.FromException like memStrDest.Write below)

            Int32 pos = _position;
            Int32 n = InternalEmulateRead(_length - _position);

            // If destination is not a memory stream, write there asynchronously:
            MemoryStream memStrDest = destination as MemoryStream;
            if (memStrDest == null)                 
                return destination.WriteAsync(_buffer, pos, n, cancellationToken);
           
            try {

                // If destination is a MemoryStream, CopyTo synchronously:
                memStrDest.Write(_buffer, pos, n);
                return Task.CompletedTask;

            } catch(Exception ex) {
                return Task.FromException(ex);
            }
        }
#endif //FEATURE_ASYNC_IO


        public override long Seek(long offset, SeekOrigin loc)
        {
            if (!_isOpen) throw new IOException("Stream is closed");

            if (offset > MemStreamMaxLength)
                throw new ArgumentOutOfRangeException(nameof(offset));
            switch (loc)
            {
                case SeekOrigin.Begin:
                {
                    int tempPosition = unchecked(_origin + (int) offset);
                    if (offset < 0 || tempPosition < _origin)
                        throw new IOException("Seek before begin");
                    _position = tempPosition;
                    break;
                }
                case SeekOrigin.Current:
                {
                    int tempPosition = unchecked(_position + (int) offset);
                    if (unchecked(_position + offset) < _origin || tempPosition < _origin)
                        throw new IOException("Seek before begin");
                    _position = tempPosition;
                    break;
                }
                case SeekOrigin.End:
                {
                    int tempPosition = unchecked(_length + (int) offset);
                    if (unchecked(_length + offset) < _origin || tempPosition < _origin)
                        throw new IOException("Seek before begin");
                    _position = tempPosition;
                    break;
                }
                default:
                    throw new ArgumentException("Invalid seek origin");
            }

            Contract.Assert(_position >= 0, "_position >= 0");
            return _position;
        }

        // Sets the length of the stream to a given value.  The new
        // value must be nonnegative and less than the space remaining in
        // the array, Int32.MaxValue - origin
        // Origin is 0 in all cases other than a MemoryStream created on
        // top of an existing array and a specific starting offset was passed 
        // into the MemoryStream constructor.  The upper bounds prevents any 
        // situations where a stream may be created on top of an array then 
        // the stream is made longer than the maximum possible length of the 
        // array (Int32.MaxValue).
        // 
        public override void SetLength(long value)
        {
            if (value < 0 || value > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Stream length");
            }
            Contract.Ensures(_length - _origin == value);
            Contract.EndContractBlock();
            EnsureWriteable();

            // Origin wasn't publicly exposed above.
            Contract.Assert(MemStreamMaxLength == Int32.MaxValue);
                // Check parameter validation logic in this method if this fails.
            if (value > (Int32.MaxValue - _origin))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Stream length");
            }

            int newLength = _origin + (int) value;
            bool allocatedNewArray = EnsureCapacity(newLength);
            if (!allocatedNewArray && newLength > _length)
                Array.Clear(_buffer, _length, newLength - _length);
            _length = newLength;
            if (_position > newLength) _position = newLength;

        }

        public virtual byte[] ToArray()
        {
            byte[] copy = new byte[Length];
            Buffer.BlockCopy(_buffer, _origin, copy, 0, _length - _origin);
            return copy;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset and length");
            Contract.EndContractBlock();

            if (!_isOpen) throw new IOException("Stream is closed");
            EnsureWriteable();

            int i = _position + count;
            // Check for overflow
            if (i < 0)
                throw new IOException("Stream too long");

            if (i > _length)
            {
                bool mustZero = _position > _length;
                if (i > _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(i);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, i - _length);
                _length = i;
            }
            if ((count <= 8) && (buffer != _buffer))
            {
                int byteCount = count;
                while (--byteCount >= 0)
                    _buffer[_position + byteCount] = buffer[offset + byteCount];
            }
            else
                Buffer.BlockCopy(buffer, offset, _buffer, _position, count);
            _position = i;

        }

#if FEATURE_ASYNC_IO
        [HostProtection(ExternalThreading = true)]
        [ComVisible(false)]
        public override Task WriteAsync(Byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset and length");
            Contract.EndContractBlock(); // contract validation copied from Write(...)

            // If cancellation is already requested, bail early
            if (cancellationToken.IsCancellationRequested) 
                return Task.FromCanceled(cancellationToken);

            try
            {
                Write(buffer, offset, count);
                return Task.CompletedTask;
            }
            catch (OperationCanceledException oce)
            {
                return Task.FromCanceled(cancellationToken);
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }
        }
#endif // FEATURE_ASYNC_IO

        public override void WriteByte(byte value)
        {
            if (!_isOpen) throw new IOException("Stream is closed");
            EnsureWriteable();

            if (_position >= _length)
            {
                int newLength = _position + 1;
                bool mustZero = _position > _length;
                if (newLength >= _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(newLength);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, _position - _length);
                _length = newLength;
            }
            _buffer[_position++] = value;

        }

        // Writes this MemoryStream to another stream.
        public virtual void WriteTo(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            Contract.EndContractBlock();

            if (!_isOpen) throw new IOException("Stream is closed");
            stream.Write(_buffer, _origin, _length - _origin);
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        private void ObjectInvariantMS() {
            Contract.Invariant(_origin >= 0);
            Contract.Invariant(_origin <= _position);
            Contract.Invariant(_length <= _capacity);
            // equivalent to _origin > 0 => !expandable, and using fact that _origin is non-negative.
            Contract.Invariant(_origin == 0 || !_expandable);
        }
#endif
    }
}