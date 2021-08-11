using System.IO;

namespace DNSProxy.Utils
{
    public static class Extension
    {
        public static ushort SwapEndian(this ushort val)
        {
            var value = (ushort)((val << 8) | (val >> 8));
            return value;
        }

        public static uint SwapEndian(this uint val)
        {
            var value = (val << 24) | ((val << 8) & 0x00ff0000) | ((val >> 8) & 0x0000ff00) | (val >> 24);
            return value;
        }

        public static byte[] GetResourceBytes(this string str, char delimiter = '.')
        {
            if (str == null) str = "";

            using (var stream = new MemoryStream(str.Length + 2))
            {
                var segments = str.Split(new[] { '.' });
                foreach (var segment in segments)
                {
                    stream.WriteByte((byte)segment.Length);
                    foreach (var currentChar in segment) stream.WriteByte((byte)currentChar);
                }

                // null delimiter
                stream.WriteByte(0x0);
                return stream.GetBuffer();
            }
        }

        public static void WriteToStream(this string str, Stream stream)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                var segments = str.Split(new[] { '.' });
                foreach (var segment in segments)
                {
                    stream.WriteByte((byte)segment.Length);
                    foreach (var currentChar in segment) stream.WriteByte((byte)currentChar);
                }
            }

            // null delimiter
            stream.WriteByte(0x0);
        }

        public static void WriteToStream(this ushort value, Stream stream)
        {
            stream.WriteByte((byte)(value & 0xFF));
            stream.WriteByte((byte)((value >> 8) & 0xFF));
        }


        public static void WriteToStream(this uint value, Stream stream)
        {
            stream.WriteByte((byte)(value & 0xFF));
            stream.WriteByte((byte)((value >> 8) & 0xFF));
            stream.WriteByte((byte)((value >> 16) & 0xFF));
            stream.WriteByte((byte)((value >> 24) & 0xFF));
        }
    }
}