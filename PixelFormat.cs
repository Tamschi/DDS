using System.Runtime.InteropServices;
using System.Text;

namespace DDS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PixelFormat
    {
        public uint Size;
        public PixelFlags Flags;
        unsafe fixed byte _fourCC[4];
        public unsafe string FourCC
        {
            get
            {
                fixed (byte* fourCCPtr = _fourCC)
                { return Encoding.ASCII.GetString(fourCCPtr, 4); }
            }
        }
        public uint RgbBitCount;
        public uint RBitMask;
        public uint GBitMask;
        public uint BBitMask;
        public uint ABitMask;
    }
}