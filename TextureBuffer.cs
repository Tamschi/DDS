using System;

namespace DDS
{
    public readonly ref struct TextureBuffer
    {
        readonly Span<byte> _data;

        public TextureBuffer(Span<byte> data) => _data = data;
    }
}