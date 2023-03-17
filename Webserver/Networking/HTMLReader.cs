using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver.Networking
{
    public class HTMLReader
    {

        public static byte[] ReadMsg(string _filename) {
            string _filepath = Environment.CurrentDirectory + HTTPServer.MSG_DIR + _filename;

            if (!File.Exists(_filepath)) return null;

            FileStream _fileStream = File.OpenRead(_filepath);
            BinaryReader _reader = new BinaryReader(_fileStream);

            Byte[] _bytes = new Byte[_fileStream.Length];
            _reader.Read(_bytes, 0, _bytes.Length);

            _reader.Close();
            _fileStream.Close();

            return _bytes;
        }

        public static byte[] ReadWeb(string _filename) {
            string _targetpath = Environment.CurrentDirectory + HTTPServer.WEB_DIR + _filename;

            string _filepath = SearchForTarget(_targetpath);
            if (String.IsNullOrEmpty(_filepath)) return null;

            FileStream _fileStream = File.OpenRead(_filepath);
            BinaryReader _reader = new BinaryReader(_fileStream);

            Byte[] _bytes = new Byte[_fileStream.Length];
            _reader.Read(_bytes, 0, _bytes.Length);

            _reader.Close();
            _fileStream.Close();

            return _bytes;
        }


        private static string SearchForTarget(string _filepath) {
            if (File.Exists(_filepath)) return _filepath;

            if (!Directory.Exists(_filepath)) return null;

            DirectoryInfo _dictonary = new DirectoryInfo(_filepath);
            FileInfo[] _files = _dictonary.GetFiles();

            foreach (FileInfo _f in _files) {
                if (_f.Name.Contains("index.htm") || _f.Name.Contains("default.htm")) return _f.FullName;
            }

            return null;
        }
    }
}
