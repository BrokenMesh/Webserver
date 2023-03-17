<a name="readme-top"></a>

<div align="center">
<h3 align="center">Hichams Web-Server</h3>

  <p align="center">
    This is a simple Web-Server based on the Dot.Net framework. It uses basic networking to create deploy a website. It also allows somewhat dynamic content. 
    <br />
    <a href="https://github.com/BrokenMesh/Webserver/issues">Report Bug</a>
    ·
    <a href="https://github.com/BrokenMesh/Webserver/issues">Request Feature</a>
  </p>
</div>


<!-- GETTING STARTED -->
## Getting Started

### Installation
1. Clone the repo
   ```sh
   git clone https://github.com/BrokenMesh/Webserver.git
   ```
2. Open the Solution
   ```sh
   Webserver.sln
   ```
3. Build Project

<!-- USAGE EXAMPLES -->
## Usage

Start a simple HTTP server on port 8080.
```C#
HTTPServer _server = new HTTPServer(8080); 
_server.Start();
```
The HTML files are located int the `Bin-Dep/root/web` folder. this folder will be copied into the build directory in a post-build event.

---
<br>

This will create a DynamicServer. This server type can change its content somewhat dynamically. Its files are located in the `Bin-Dep/root/dyn` folder.

```C#
DynamicServer _server = new DynamicServer(8080); 
```

This will add an Element. If the server finds an Element Pointer with the same nam, it will call the lambda function and replace the Pointer with what the lambda returns. 
```C#
_server.AddElement("Title", (string _action) => {
      return "<h1>Hello, World!</h1>";
});
```

Element pointer are defined like this: 
```HTML
<body>
  $_eptr="Title" <!-- This is the Element Pointer -->

  <p>This is a Text</p>
<body>
```

<!-- LICENSE -->
## License

Distributed under the GNU GENERAL PUBLIC LICENSE License. See `LICENSE.txt` for more information.

<!-- CONTACT -->
## Contact

Hicham El-Kord - hichamelkord@gmail.com

Project Link: [https://github.com/BrokenMesh/Webserver](https://github.com/github_username/repo_name)


<p align="right">(<a href="#readme-top">back to top</a>)</p>





















