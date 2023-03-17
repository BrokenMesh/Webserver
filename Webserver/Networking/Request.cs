using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace Webserver.Networking
{
    public class Request
    {
        public string Type { get; private set; }
        public string URL { get; private set; }
        public string Action { get; private set; }
        public string Host { get; private set; }
        public string Content { get; private set; }

        public Dictionary<string, string> AdditionalData { get; private set; }

        public Request(string _type, string _url, string _host, string _action, string _content, Dictionary<string, string> _additionalData) {
            Type = _type;
            URL = _url;
            Host = _host;
            Action = _action;
            Content = _content;
            AdditionalData = _additionalData;
        }

        public static Request GetRequest(string _request) {
            if (string.IsNullOrEmpty(_request)) return null;

            // Read Main Values
            string[] _tokens = _request.Split(new char[] { ' ', '\n' });
            string _type = _tokens[0];
            string _url = "";
            string _action = "";

            if (_type == "POST") 
                Console.WriteLine();

            if (_tokens[1].Contains("?")) {
                string[] _pathSections = _tokens[1].Split("?");
                _url = _pathSections[0];
                _action = _pathSections[1];
            } else {
                _url = _tokens[1];
            }
            string _host = _tokens[4];

            // Read Additional Values
            Dictionary<string, string> _additionalData = new Dictionary<string, string>();
            string[] _lines = _request.Split(new char[] { '\n' });

            for (int i = 2; i < _lines.Length; i++) {
                if (!_lines[i].Contains(":")) continue;
                if (string.IsNullOrEmpty(_lines[i].Trim())) break;

                string _name = _lines[i].Substring(0, _lines[i].IndexOf(":")).Trim();
                string _value = _lines[i].Substring(_lines[i].IndexOf(":")+1).Trim();

                _additionalData.Add(_name, _value);
            }

            // Read Content
            string _content = "";

            if (_additionalData.ContainsKey("Content-Length")) {
                int _contentLength;
                bool _parseResult = int.TryParse(_additionalData["Content-Length"], out _contentLength);

                if (_parseResult) { 
                    _content = _request.Substring(_request.Length - (_contentLength+1));            
                }
            }

            return new Request(_type, _url, _host, _action, _content, _additionalData);
        }

    }
}