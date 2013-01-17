using System;
using System.Text;

namespace MetaWeblogSharp.XmlRPC
{
    public class Service
    {
        public String URL;

        public Service(string url)
        {
            this.URL = url;
        }

        public MethodResponse ExecuteRaw(MethodCall methodcall)
        {
            var doc = methodcall.CreateDocument();
            var request = System.Net.WebRequest.Create(this.URL);
            request.Method = "POST";
            var content = doc.ToString();
            var byteArray = Encoding.UTF8.GetBytes(content);
            request.ContentType = "text/xml";
            request.ContentLength = byteArray.Length;

            using (var webpageStream = request.GetRequestStream())
            {
                webpageStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var webResponse = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                {
                    string webpageContent = reader.ReadToEnd();
                    var response = new MethodResponse(webpageContent);
                    return response;
                }
            }
        }
    }
}