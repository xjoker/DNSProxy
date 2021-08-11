using System;
using System.IO;
using DNSProxy.Utils;

namespace DNSProxy.DNS
{
    public class Question
    {
        public ResourceClass Class;
        public string Name;
        public ResourceType Type;

        public void WriteToStream(Stream stream)
        {
            var name = Name.GetResourceBytes();
            stream.Write(name, 0, name.Length);

            // Type
            stream.Write(BitConverter.GetBytes(((ushort)Type).SwapEndian()), 0, 2);

            // Class
            stream.Write(BitConverter.GetBytes(((ushort)Class).SwapEndian()), 0, 2);
        }
    }
}