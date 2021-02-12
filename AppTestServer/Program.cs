using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace AppTestServer
{
    class Program
    {
        private static EventWaitHandle _waitHandler = new EventWaitHandle(false, EventResetMode.ManualReset);

        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 9091));
            listener.Start();
            _ = _AcceptTcpClient(listener);

            Console.WriteLine("Ожидание подключения....");
            _waitHandler.WaitOne();
            Console.WriteLine("Клиент подключен");
            
            while (true)
            {
                string ln = Console.ReadLine();
                StreamWriter sw = new StreamWriter(_tcpClient.GetStream());
                sw.WriteLine(ln);
            }
        }

        static async Task _AcceptTcpClient(TcpListener listener)
        {
            await Task.Delay(1);
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                _ = _ProcessTcpClient(tcpClient);
                await Task.Delay(100);
            }
        }

        private static TcpClient _tcpClient;

        static async Task _ProcessTcpClient(TcpClient tcp)
        {
            _tcpClient = tcp;
            await Task.Delay(10);
            NetworkStream netStream = tcp.GetStream();
            StreamReader reader = new StreamReader(netStream);
            while (true)
            {
                if (netStream.DataAvailable)
                {
                    string dataString = reader.ReadLine();
                    Console.WriteLine($"[CLIENT] {dataString}");
                }

                await Task.Delay(100);
            }
        }


        
    }
}
