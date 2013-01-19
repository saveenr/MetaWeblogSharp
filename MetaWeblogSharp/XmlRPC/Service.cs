using System;
using System.IO;
using System.Text;

namespace MetaWeblogSharp.XmlRPC
{
    public class Service
    {
        public String URL { get; private set; }

        public Service(string url)
        {
            this.URL = url;
        }

        public MethodResponse Execute(MethodCall methodcall)
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
                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        throw new XmlRPCException("Response Stream is unexpectedly null");
                    }

                    using (var reader = new System.IO.StreamReader(responseStream))
                    {
                        string webpageContent = reader.ReadToEnd();
                        var response = new MethodResponse(webpageContent);
                        return response;
                    }                    
                }
            }
        }
    }
}