using Flexlive.CQP.Framework;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Flexlive.CQP.CSharpPlugins.Demo
{
    public class socket
    {
        static Socket serverSocket;
        private static byte[] read = new byte[4096];
        public static void start_socket()
        {
            IPAddress ip = IPAddress.Parse(MyPlugin.ipaddress);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, MyPlugin.Port));
            serverSocket.Listen(10);    
            Thread myThread = new Thread(Read);
            myThread.Start();
        }
        private static void Read()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Socket myClientSocket = (Socket)clientSocket;
                int a;
                a = myClientSocket.Receive(read);
                MyPlugin.read_text = Encoding.Default.GetString(read, 0, a);
                if (MyPlugin.read_text != "")
                {
                    CQ.SendGroupMessage(MyPlugin.GroupSet, MyPlugin.read_text);
                }
                MyPlugin.read_text = "";
                if (MyPlugin.text != "")
                {
                    clientSocket.Send(Encoding.Default.GetBytes(MyPlugin.text));
                }
                MyPlugin.text = "";
                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
            }
        }
    }
}
