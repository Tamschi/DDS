using System;

namespace DDS
{
    [Flags]
    public enum Flags : uint
    {
        Caps = 1,
        Height = 2,
        Width = 4,
        Pitch = 8,
        PixelFormat = 0x1000,
        MipmapCount = 0x20000,
        LinearSize = 0x80000,
        Depth = 0x800000
    }
}