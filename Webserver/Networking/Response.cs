using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Webserver.Networking.Dynamic;

namespace Webserver.Networking
{
    public class Response
    {
        private string type;
        private string serverName;
        private string status;
        private string mime;

        private Byte[] data;

        public Response(string _type, string _servername, string _status, string _mime, Byte[] _data) {
            type = _type;
            serverName = _servername;
            status = _status;
            mime = _mime;
            data = _data;
        }

        public byte[] Generate() {
            string _headstr = "";
            _headstr += HTTPServer.VERSION;
            _headstr += $" {status} \r\n";
            _headstr += $"Server: {serverName} \r\n";
            _headstr += $"Content-Type: {mime} \r\n";
            _headstr += $"Accept-Ranges: bytes \r\n";
            _headstr += $"Content-Lenght: {data.Length} \r\n\n";

            byte[] _head = Encoding.ASCII.GetBytes(_headstr);

            byte[] _msg = new byte[_head.Length + data.Length];
            System.Buffer.BlockCopy(_head, 0, _msg, 0, _head.Length);
            System.Buffer.BlockCopy(data, 0, _msg, _head.Length, data.Length);

            return _msg;
        }

        public static Response From(Request _request) {
            if (_request == null) return MakeNullRequest();
            if (_request.Type != "GET") {
                return MakeMethodNotAllowed(); 
            }

            byte[] _data = HTMLReader.ReadWeb(_request.URL);
            if (_data == null) return MakePageNotFound();

            return new Response("GET", HTTPServer.SERVER_NAME, "200 OK", "text/html", _data);
        }

        public static Response DynamicFrom(Request _request, Dictionary<string, Func<string, string>> _activeElements) {
            if (_request == null) return MakeNullRequest();
            if (_request.Type != "GET") return MakeMethodNotAllowed();

            byte[] _data = DHTMLPage.ReadPage(_request.URL, _request.Action, _activeElements);
            if (_data == null) return MakePageNotFound();

            return new Response("GET", HTTPServer.SERVER_NAME, "200 OK", "text/html", _data);
        }

        private static Response MakeNullRequest() {
            return new Response("GET", HTTPServer.SERVER_NAME, "400 Bad Requset", "text/html", HTMLReader.ReadMsg("/400.html"));
        }

        private static Response MakePageNotFound() {
            return new Response("GET", HTTPServer.SERVER_NAME, "404 Page not found", "text/html", HTMLReader.ReadMsg("/404.html"));
        }

        private static Response MakeMethodNotAllowed() {
            return new Response("GET", HTTPServer.SERVER_NAME, "405 MethodNotAllowed", "text/html", HTMLReader.ReadMsg("/405.html"));
        }

    }
}
