using SpanUtils;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DDS
{
    public readonly ref struct DDS
    {
        readonly Span<byte> _data;
        public DDS(Span<byte> data)
        {
            _data = data;
            for (int i = 0; i < _magic.Length; i++)
            {
                if (_data[i] != _magic[i])
                { throw new ArgumentException("Wrong magic! Expected file to start with DDS.", nameof(data)); }
            }
        }

        static readonly byte[] _magic = Encoding.ASCII.GetBytes("DDS");
        public Variant Variant => (Variant)_data.View<byte>(_magic.Length);
        static readonly int _headerOffset = _magic.Length + sizeof(byte);
        static readonly int _headerLenth = Marshal.SizeOf<Header>();
        public ref Header Header => ref _data.View<Header>(_headerOffset);

        public Enumerator GetEnumerator() => new Enumerator(_data.Slice(_headerOffset + _headerLenth), in Header);
        public ref struct Enumerator
        {
            readonly Span<byte> _data;
            readonly uint[] _mipLengths;
            readonly int _layers;

            int _layer;

            uint _LayerLength
            {
                get
                {
                    uint result = 0;
                    foreach (var value in _mipLengths)
                    { result += value; }
                    return result;
                }
            }

            public Enumerator(Span<byte> data, in Header header)
            {
                _data = data;
                _layer = -1;

                if (header.Capabilities.HasFlag(Capabilities.Cubemap))
                {
                    _layers = 0;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapNegativeX)) _layers++;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapPositiveX)) _layers++;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapNegativeY)) _layers++;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapPositiveY)) _layers++;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapNegativeZ)) _layers++;
                    if (header.Capabilities.HasFlag(Capabilities.CubemapPositiveZ)) _layers++;
                }
                else _layers = 1;

                var mips = header.MipmapCount;
                _mipLengths = new uint[mips];
                for (int m = 0; m < mips; m++)
                {
                    var width = Math.Max(header.Width >> m, 1);
                    var height = Math.Max(header.Height >> m, 1);

                    if (header.Capabilities.HasFlag(Capabilities.Complex))
                    {
                        var fourCC = header.PixelFormat.FourCC;
                        if (fourCC.StartsWith("DXT"))
                        {
                            width.IncreaseToMultiple(4);
                            height.IncreaseToMultiple(4);
                            if (fourCC == "DXT1") _mipLengths[m] = width * height / 2;
                            else _mipLengths[m] = width * height;
                        }
                        else throw new NotImplementedException();
                    }
                    else if (header.PixelFormat.RgbBitCount != 0)
                    {
                        var bits = width * height * header.PixelFormat.RgbBitCount;
                        bits.IncreaseToMultiple(8);
                        _mipLengths[m] = bits / 8;
                    }
                    else throw new NotImplementedException();
                }
            }

            public Layer Current => new Layer(_data.Slice(_layer * (int)_LayerLength, (int)_LayerLength), _mipLengths);

            public bool MoveNext()
            {
                _layer++;
                switch (_layer.CompareTo(_layers))
                {
                    case var x when x < 0: return true;
                    case var x when x == 0: return false;
                    default /* var x when x > 0 */: throw new InvalidOperationException("Tried to move beyond layer count.");
                }
            }
        }
    }
}
