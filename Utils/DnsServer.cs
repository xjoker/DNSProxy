using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using DNSProxy.DNS;

namespace DNSProxy.Utils
{
    public class DnsServer
    {
        private readonly ushort _port;
        private UdpListener _udpListener;

        internal DnsServer(ushort port = 53)
        {
            _port = port;
        }

        public void Initialize()
        {
            _udpListener = new UdpListener();
            _udpListener.OnRequest += ProcessDnsRequest;
        }

        public void Start()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            _udpListener.StartListenerAsync(ipEndPoint);
        }

        public void Stop()
        {
            _udpListener.Stop();
        }

        private void ProcessDnsRequest(UdpReceiveResult receiveResult, UdpClient udpClient)
        {
            if (DnsProtocol.TryParse(receiveResult.Buffer, out var dnsModel))
            {
                Debug.WriteLine(dnsModel.Questions.First().Name);
                var proxy = new UdpClient(53);
                proxy.Client.ReceiveTimeout = 3000;
                proxy.Connect(IPAddress.Parse("114.114.114.114"), 53);
                proxy.Send(receiveResult.Buffer, receiveResult.Buffer.Length);
                var dnsReceiveAsync = proxy.ReceiveAsync().Result;
                proxy.Close();


                udpClient.SendAsync(dnsReceiveAsync.Buffer, dnsReceiveAsync.Buffer.Length,
                    receiveResult.RemoteEndPoint);
                udpClient.Client.SendTimeout = 3000;
                udpClient.Close();
            }
        }
    }
}