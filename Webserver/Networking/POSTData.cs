using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver.Networking
{
    public class POSTData
    {
        public string Path { get; private set; }

        private Dictionary<string, string> postData;

        public POSTData(Dictionary<string, string> _postData, string _path) {
            postData = _postData;
            Path = _path;    
        }

        public string Get(string _name) {
            if (!postData.ContainsKey(_name)) return "";
            return postData[_name];
        }

        public static POSTData From(string _content, string _path) {
            _content = _content.Trim().Replace("+", " ");

            string[] _values = _content.Split(new char[] { '&' });
            Dictionary<string, string> _postData = new Dictionary<string, string>();

            foreach (string _i in _values) {
                if (string.IsNullOrEmpty(_i.Trim())) continue;
                if (!_i.Contains("=")) continue;

                string _name = _i.Substring(0, _i.IndexOf("=")).Trim();
                string _value = _i.Substring(_i.IndexOf("=") + 1).Trim();

                _postData.Add(_name, _value);
            }

            return new POSTData(_postData, _path);
        }

    }
}
