using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Webserver.Networking.Dynamic
{
    public class DHTMLPage
    {
        public static byte[] ReadPage(string _filename, string _action, Dictionary<string, Func<string, string>> _activeElements) {
            string _targetpath = Environment.CurrentDirectory + DynamicServer.DYN_DIR + _filename;

            string _filepath = SearchForTarget(_targetpath);
            if (string.IsNullOrEmpty(_filepath)) return null;

            return Generate(File.ReadAllText(_filepath), _action, _activeElements);
        }

        private static byte[] Generate(string _mainPage, string _action, Dictionary<string, Func<string, string>> _activeElements) {
            string _outputPage = _mainPage;

            List<string> _ptrs = new List<string>();

            foreach (string _s in _mainPage.Split("$")) {
                if (!_s.StartsWith("_eptr=\"")) continue;
                string _pagePtrName = _s.Split("\"")[1].Trim();
                _ptrs.Add(_pagePtrName);
            }

            foreach (string _e in _ptrs) {
                string _content = "";

                if (_activeElements.ContainsKey(_e)) { 
                    _content = _activeElements[_e].Invoke(_action);
                }

                _outputPage = _outputPage.Replace($"$_eptr=\"{_e}\"", _content);
            }

            return Encoding.ASCII.GetBytes(_outputPage);
        }


        private static string SearchForTarget(string _filepath)
        {
            if (File.Exists(_filepath)) return _filepath;

            if (!Directory.Exists(_filepath)) return null;

            DirectoryInfo _dictonary = new DirectoryInfo(_filepath);
            FileInfo[] _files = _dictonary.GetFiles();

            foreach (FileInfo _f in _files)
            {
                if (_f.Name.Contains("index.dhtml") || _f.Name.Contains("default.dhtml")) return _f.FullName;
            }

            return null;
        }
    }
}
