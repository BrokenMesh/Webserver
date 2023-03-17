using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Webserver.Networking
{
    public class HTTPServer
    {
        public const string SERVER_NAME = "DEMO_SERVER";
        public const string VERSION = "HTTP/1.1";
        public const string MSG_DIR = "/root/msg";
        public const string WEB_DIR = "/root/web";

        private bool isRunning = false;
        private TcpListener tcpListener;

        public HTTPServer(int _port) {
            tcpListener = new TcpListener(System.Net.IPAddress.Any, _port);
        }

        public void Start() {
            Thread _serverThread = new Thread(new ThreadStart(Run));
            _serverThread.Start();
        }

        private void Run() {
            isRunning = true;
            tcpListener.Start();

            Console.WriteLine("Server has started!");

            while (isRunning) {
                Console.WriteLine("Waiting for connection ...");
                TcpClient _client = tcpListener.AcceptTcpClient();

                Console.WriteLine($"Client '{_client.Client.RemoteEndPoint}' had connected!");

                HandleClient(_client);
                _client.Close(); 
            }

            isRunning = false;
            tcpListener.Stop();

            Console.WriteLine("Server has stopped");
        }

        private void HandleClient(TcpClient _client) {
            StreamReader _reader = new StreamReader(_client.GetStream());

            string _msg = "";
            while (_reader.Peek() != -1) {
                _msg += _reader.ReadLine() + "\n";
            }

            Debug.WriteLine($"Request: \n{_msg}");

            Response _response = BuildResponse(Request.GetRequest(_msg));

            byte[] _responsedata = _response.Generate();

            Debug.WriteLine(Encoding.ASCII.GetString(_responsedata));

            _client.GetStream().Write(_responsedata, 0, _responsedata.Length);
        }

        public virtual Response BuildResponse(Request _request) { 
            return Response.From(_request);
        }

    }
}
