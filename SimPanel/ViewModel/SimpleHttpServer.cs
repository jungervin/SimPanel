using SimPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace SimPanel.ViewModel
{
    public class SimpleHttpServer : HttpServer
    {
        public SimpleHttpServer(int port) : base(System.Net.IPAddress.Any, port)
        {
            this.DocumentRootPath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "HTML");
            this.OnGet += SimpleHttpServer_OnGet;

            this.AddWebSocketService<WSServer>("/sim");
            this.AddWebSocketService<WSVariables>("/variables");
        }

        private void SimpleHttpServer_OnGet(object sender, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;

            var path = req.RawUrl;
            if (path == "/")
                path += "index.html";

            byte[] contents;
            if (!e.TryReadFile(path, out contents))
            {
                res.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }
            res.ContentType = MimeTypes.Mappings[Path.GetExtension(path)]; 

            //if (path.EndsWith(".html"))
            //{
            //    res.ContentType = "text/html";
            //    res.ContentEncoding = Encoding.UTF8;
            //}
            //else if (path.EndsWith(".js"))
            //{
            //    res.ContentType = "application/javascript";
            //    res.ContentEncoding = Encoding.UTF8;
            //}
            //else if (path.EndsWith(".json"))
            //{
            //    res.ContentType = "application/json";
            //    res.ContentEncoding = Encoding.UTF8;
            //}

            res.ContentLength64 = contents.LongLength;
            res.Close(contents, true);


        }
    }
}
