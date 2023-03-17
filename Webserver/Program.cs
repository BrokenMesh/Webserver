// Webserver - 17/3/23 - Hicham

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webserver.Networking;
using Webserver.Networking.Dynamic;

// --- Info ---
// This project has a post-build event that copy's the files from the folder 
// Bin-Dep to the build target folder. It uses the 'xcopy' cmdlet which may 
// not work on other platforms then windows.  
//

namespace Webserver
{
    public class Program
    {
        public static void Main() {

            Console.WriteLine("Server is starting ...");
            Blog.Start();

        }
    }
}
