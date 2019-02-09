using System;

namespace DDS
{
    public readonly ref struct Mip
    {
        public Span<byte> Data { get; }

        public Mip(Span<byte> data) => Data = data;
    }
}