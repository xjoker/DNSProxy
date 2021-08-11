using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace DNSProxy.Utils
{
    public delegate void OnRequestHandler(UdpReceiveResult args, UdpClient udpClient);

    public class UdpListener
    {
        private bool IsRun;

        /// <summary>
        ///     消息处理委托
        /// </summary>
        public OnRequestHandler OnRequest;

        public async void StartListenerAsync(IPEndPoint endPoint)
        {
            IsRun = true;
            while (true)
            {
                if (!IsRun) break;
                try
                {
                    try
                    {
                        while (true)
                        {
                            if (!IsRun) break;
                            using var listener = new UdpClient(endPoint);
                            var receiveResult = await listener.ReceiveAsync();
                            if (receiveResult.Buffer.Length > 0) OnRequest?.Invoke(receiveResult, listener);
                        }
                    }
                    catch (SocketException e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                catch (Exception e)
                {
                    // 当异常后不会终止接收信息
                    Debug.WriteLine(e);
                }
            }
        }

        public void Stop()
        {
            IsRun = false;
            //listener.Close();
        }
    }
}