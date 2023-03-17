using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webserver.Networking;
using Webserver.Networking.Dynamic;

namespace Webserver
{

    // This is an example, showing how you would use this web-server.
    public class Blog
    {
        public const string BLOG_PATH = "/blog/data.txt";

        public static void Start() {
            List<Post> _posts = LoadPosts();


            // This is the Template which will be inserted in the source html file.
            string _postFrame =
                "<div class=\"element\">" +
                "<p><b>_{name}</b> <i>_{date}</i></p>" +
                "<p>_{content}</p>" +
                "</div>";

            DynamicServer _server = new DynamicServer(8080); 

            // This adds an Element to the Elements list of the server.
            // When the server is reading the source HTML file and finds a Element definition/pointer like this $_eptr="Elements"
            // it will call the given lambda function and insert the text that the lambda function returns into the location
            // of the Element pointer.
            _server.AddElement("Elements", (string _action) => {
                string _content = "";

                // This creates the html for the post-list by inserting the values into the '_postFrame' template.
                foreach (Post _p in _posts) {
                    _content += _postFrame.Replace("_{name}", _p.name).Replace("_{date}", _p.date).Replace("_{content}", _p.content);
                }

                return _content;
            });

            // This is called when we are tying to create a post. 
            _server.AddElement("State", (string _action) => {     
                // The action is the value int the URL after the path. The path and action are separated by a '?'.
                //  - http://localhost:8080/blog/index.dhtml?name=abc$file=123 
                // here the 'name=abc$file=123' would the the action.
                _action = _action.Replace("%20", " "); 
                
                if (!_action.Contains("$")) return "Failed to create post!";

                string[] _values = _action.Trim().Split("$");
                if(_values.Length != 2) return "Failed to create post!";

                string _name = _values[0];
                string _post = _values[1];

                foreach (Post _p in _posts) {
                    if(_p.name == _name && _p.content == _post)
                        return "Failed to create post!";
                }

                _posts.Add(new Post(_name, _post, DateTime.Now.ToString("dd.MM.yyyy - HH:mm")));
                SavePosts(_posts);

                return "Succsesfully created Post";
            });

            _server.Start(); // Starts the Server (localhost:8080)
        }
        
        private static List<Post> LoadPosts() {
            string _filepath = Environment.CurrentDirectory + Blog.BLOG_PATH;

            if (!File.Exists(_filepath)) return new List<Post>();

            List<Post> _posts = new List<Post>();

            string _file = File.ReadAllText(_filepath);
            foreach (string _line in _file.Split("\n")) {
                if (!_line.Contains(";")) continue;

                string[] _values = _line.Split(';');
                if (_values.Length != 3) continue;

                _posts.Add(new Post(_values[0].Trim(), _values[1].Trim(), _values[2].Trim()));
            }

            return _posts;
        }

        private static void SavePosts(List<Post> _posts) {
            string _filepath = Environment.CurrentDirectory + Blog.BLOG_PATH;

            string _output = "";

            foreach (Post _post in _posts) {
                _output += $"{_post.name}; {_post.content}; {_post.date}\n";
            }

            File.WriteAllText(_filepath, _output);
        }

    }

    struct Post {
        public string name;
        public string content;
        public string date;

        public Post(string name, string content, string date) {
            this.name = name;
            this.content = content;
            this.date = date;
        }
    }
}
