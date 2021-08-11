﻿using System;
using System.IO;
using DNSProxy.Utils;

namespace DNSProxy.DNS
{
    public class ResourceRecord
    {
        public string Name { get; set; }
        public uint TTL { get; set; }
        public ResourceClass Class { get; set; }
        public ResourceType Type { get; set; }
        public RData RData { get; set; }
        public ushort DataLength { get; set; }

        /// <summary>Serialize resource to stream according to RFC1034 format</summary>
        /// <param name="stream"></param>
        public void WriteToStream(Stream stream)
        {
            Name.WriteToStream(stream);
            ((ushort)Type).SwapEndian().WriteToStream(stream);
            ((ushort)Class).SwapEndian().WriteToStream(stream);
            TTL.SwapEndian().WriteToStream(stream);

            if (RData != null)
            {
                RData.Length.SwapEndian().WriteToStream(stream);
                RData.WriteToStream(stream);
            }
            else
            {
                // no RDATA write (ushort) DataLength=0
                stream.WriteByte(0x00);
                stream.WriteByte(0x00);
            }
        }

        public void Dump()
        {
            Console.WriteLine("ResourceName:   {0}", Name);
            Console.WriteLine("ResourceType:   {0}", Type);
            Console.WriteLine("ResourceClass:  {0}", Class);
            Console.WriteLine("TimeToLive:     {0}", TTL);
            Console.WriteLine("DataLength:     {0}", DataLength);

            if (RData != null) RData.Dump();
        }
    }
}