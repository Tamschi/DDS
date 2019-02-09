using System.Runtime.InteropServices;

namespace DDS
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header
    {
        public uint Size;
        public Flags Flags;
        public uint Height;
        public uint Width;
        public uint LinearSize;
        public uint Depth;
        public uint MipmapCount;
        unsafe fixed byte _reserved1[sizeof(uint) * 11];
        public PixelFormat PixelFormat;
        public Capabilities Capabilities;
        unsafe fixed byte _moreCapabilities[8];
        unsafe fixed byte _reserved2[4];
    }
}