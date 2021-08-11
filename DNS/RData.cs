using System;
using System.IO;
using System.Net;
using DNSProxy.Utils;

namespace DNSProxy.DNS
{
    public abstract class RData
    {
        public abstract ushort Length { get; }
        public abstract void Dump();

        public abstract void WriteToStream(Stream stream);
    }

    public class ANameRData : RData
    {
        public IPAddress Address { get; set; }

        public override ushort Length => 4;

        public static ANameRData Parse(byte[] bytes, int offset, int size)
        {
            var aname = new ANameRData();
            var addressBytes = BitConverter.ToUInt32(bytes, offset);
            aname.Address = new IPAddress(addressBytes);
            return aname;
        }

        public override void WriteToStream(Stream stream)
        {
            var bytes = Address.GetAddressBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        public override void Dump()
        {
            Console.WriteLine("Address:   {0}", Address);
        }
    }

    public class CNameRData : RData
    {
        public string Name { get; set; }

        public override ushort Length =>
            // dots replaced by bytes
            // + 1 segment prefix
            // + 1 null terminator
            (ushort)(Name.Length + 2);

        public static CNameRData Parse(byte[] bytes, int offset, int size)
        {
            var cname = new CNameRData();
            cname.Name = DnsProtocol.ReadString(bytes, ref offset);
            return cname;
        }

        public override void WriteToStream(Stream stream)
        {
            Name.WriteToStream(stream);
        }

        public override void Dump()
        {
            Console.WriteLine("CName:   {0}", Name);
        }
    }

    public class DomainNamePointRData : RData
    {
        public string Name { get; set; }

        public override ushort Length =>
            // dots replaced by bytes
            // + 1 segment prefix
            // + 1 null terminator
            (ushort)(Name.Length + 2);

        public static DomainNamePointRData Parse(byte[] bytes, int offset, int size)
        {
            var domainName = new DomainNamePointRData();
            domainName.Name = DnsProtocol.ReadString(bytes, ref offset);
            return domainName;
        }

        public override void WriteToStream(Stream stream)
        {
            Name.WriteToStream(stream);
        }

        public override void Dump()
        {
            Console.WriteLine("DName:   {0}", Name);
        }
    }

    public class NameServerRData : RData
    {
        public string Name { get; set; }

        public override ushort Length =>
            // dots replaced by bytes
            // + 1 segment prefix
            // + 1 null terminator
            (ushort)(Name.Length + 2);

        public static NameServerRData Parse(byte[] bytes, int offset, int size)
        {
            var nsRdata = new NameServerRData();
            nsRdata.Name = DnsProtocol.ReadString(bytes, ref offset);
            return nsRdata;
        }

        public override void WriteToStream(Stream stream)
        {
            Name.WriteToStream(stream);
        }

        public override void Dump()
        {
            Console.WriteLine("NameServer:   {0}", Name);
        }
    }

    public class StatementOfAuthorityRData : RData
    {
        public string PrimaryNameServer { get; set; }
        public string ResponsibleAuthoritativeMailbox { get; set; }
        public uint Serial { get; set; }
        public uint RefreshInterval { get; set; }
        public uint RetryInterval { get; set; }
        public uint ExpirationLimit { get; set; }
        public uint MinimumTTL { get; set; }

        public override ushort Length =>
            // dots replaced by bytes
            // + 1 segment prefix
            // + 1 null terminator
            (ushort)(PrimaryNameServer.Length + 2 + ResponsibleAuthoritativeMailbox.Length + 2 + 20);

        public static StatementOfAuthorityRData Parse(byte[] bytes, int offset, int size)
        {
            var soaRdata = new StatementOfAuthorityRData();
            soaRdata.PrimaryNameServer = DnsProtocol.ReadString(bytes, ref offset);
            soaRdata.ResponsibleAuthoritativeMailbox = DnsProtocol.ReadString(bytes, ref offset);
            soaRdata.Serial = DnsProtocol.ReadUint(bytes, ref offset).SwapEndian();
            soaRdata.RefreshInterval = DnsProtocol.ReadUint(bytes, ref offset).SwapEndian();
            soaRdata.RetryInterval = DnsProtocol.ReadUint(bytes, ref offset).SwapEndian();
            soaRdata.ExpirationLimit = DnsProtocol.ReadUint(bytes, ref offset).SwapEndian();
            soaRdata.MinimumTTL = DnsProtocol.ReadUint(bytes, ref offset).SwapEndian();
            return soaRdata;
        }

        public override void WriteToStream(Stream stream)
        {
            PrimaryNameServer.WriteToStream(stream);
            ResponsibleAuthoritativeMailbox.WriteToStream(stream);
            Serial.SwapEndian().WriteToStream(stream);
            RefreshInterval.SwapEndian().WriteToStream(stream);
            RetryInterval.SwapEndian().WriteToStream(stream);
            ExpirationLimit.SwapEndian().WriteToStream(stream);
            MinimumTTL.SwapEndian().WriteToStream(stream);
        }

        public override void Dump()
        {
            Console.WriteLine("PrimaryNameServer:               {0}", PrimaryNameServer);
            Console.WriteLine("ResponsibleAuthoritativeMailbox: {0}", ResponsibleAuthoritativeMailbox);
            Console.WriteLine("Serial:                          {0}", Serial);
            Console.WriteLine("RefreshInterval:                 {0}", RefreshInterval);
            Console.WriteLine("RetryInterval:                   {0}", RetryInterval);
            Console.WriteLine("ExpirationLimit:                 {0}", ExpirationLimit);
            Console.WriteLine("MinimumTTL:                      {0}", MinimumTTL);
        }
    }
}