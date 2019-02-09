using System;

namespace DDS
{
    public readonly ref struct Layer
    {
        readonly Span<byte> _data;
        readonly uint[] _mipLengths;

        public Layer(Span<byte> data, uint[] mipLengths)
        {
            _data = data;
            _mipLengths = mipLengths;
        }

        public Enumerator GetEnumerator() => new Enumerator(_data, _mipLengths);
        public ref struct Enumerator
        {
            readonly Span<byte> _data;
            readonly uint[] _lengths;

            uint _currentOffset;
            int _currentIndex;

            public Enumerator(Span<byte> data, uint[] mipLengths)
            {
                _data = data;
                _lengths = mipLengths;
                _currentOffset = 0;
                _currentIndex = -1;
            }

            public Mip Current => new Mip(_data.Slice((int)_currentOffset, (int)_lengths[_currentIndex]));

            public bool MoveNext()
            {
                if (_currentIndex != -1) _currentOffset += _lengths[_currentIndex];
                _currentIndex++;
                switch (_currentIndex.CompareTo(_lengths.Length))
                {
                    case var x when x < 0: return true;
                    case var x when x == 0: return false;
                    default /* var x when x > 0 */: throw new InvalidOperationException("Tried to move beyond mip map count.");
                }
            }
        }
    }
}