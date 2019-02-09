using System;
using static DDS.Utils;

namespace DDS
{
    [Flags]
    public enum Capabilities : ulong
    {
        // first int
        Complex = 0x8,
        Mipmap = 0x400000,
        Texture = 0x1000,
        // second int
        Cubemap = 0x200ul << INT_BITS,
        CubemapPositiveX = 0x400ul << INT_BITS,
        CubemapNegativeX = 0x800ul << INT_BITS,
        CubemapPositiveY = 0x1000ul << INT_BITS,
        CubemapNegativeY = 0x2000ul << INT_BITS,
        CubemapPositiveZ = 0x4000ul << INT_BITS,
        CubemapNegativeZ = 0x8000ul << INT_BITS,
        Volume = 0x200000ul << INT_BITS
    }
}