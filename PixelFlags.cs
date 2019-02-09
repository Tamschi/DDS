using System;

namespace DDS
{
    [Flags]
    public enum PixelFlags
    {
        AlphaPixels = 1,
        Alpha = 2,
        FourCC = 4,
        RGB = 0x40,
        YUV = 0x200,
        Luminance = 0x2000
    }
}