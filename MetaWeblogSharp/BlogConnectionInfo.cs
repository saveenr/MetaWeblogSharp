namespace MetaWeblogSharp
{
    public class BlogConnectionInfo
    {
        public string BlogURL { get; set; }
        public string MetaWeblogURL { get; set; }
        public string BlogID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static BlogConnectionInfo Load(string filename)
        {
            var doc = System.Xml.Linq.XDocument.Load(filename);
            var root = doc.Root;

            string blogurl = root.GetElementString("blogurl");
            string blogId = root.GetElementString("blogid");
            string metaWeblogUrl = root.GetElementString("metaweblog_url");
            string username = root.GetElementString("username");
            string password = root.GetElementString("password");

            var coninfo = new BlogConnectionInfo(blogurl, metaWeblogUrl, blogId, username, password);

            return coninfo;
        }

        public void Save(string filename)
        {
            var doc = new System.Xml.Linq.XDocument();
            var p = new System.Xml.Linq.XElement("blogconnectioninfo");
            doc.Add(p);
            p.Add(new System.Xml.Linq.XElement("blogurl", this.BlogURL));
            p.Add(new System.Xml.Linq.XElement("blogid", this.BlogID));
            p.Add(new System.Xml.Linq.XElement("metaweblog_url", this.MetaWeblogURL));
            p.Add(new System.Xml.Linq.XElement("username", this.Username));
            p.Add(new System.Xml.Linq.XElement("password", this.Password));
            doc.Save(filename);
        }
        
        public BlogConnectionInfo(string blogurl, string metaweblogurl, string blogid, string username, string password)
        {
            this.BlogURL = blogurl;
            this.BlogID = blogid;
            this.MetaWeblogURL = metaweblogurl;
            this.Username = username;
            this.Password = password;
        }
    }
}