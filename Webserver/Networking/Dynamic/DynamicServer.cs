using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver.Networking.Dynamic
{
    public class DynamicServer : HTTPServer
    {
        public const string DYN_DIR = "/root/dyn";

        private Dictionary<string, Func<string, string>> activeElement;

        public DynamicServer(int _port) : base(_port) {
            activeElement = new Dictionary<string, Func<string, string>>();
        }

        public override Response BuildResponse(Request _request) {
            return Response.DynamicFrom(_request, activeElement);
        }

        public void AddElement(string _name, Func<string, string> _func) {
            activeElement.Add(_name, _func);
        }

    }
}
