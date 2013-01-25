namespace MetaWeblogSharp
{
    public class BlogConnectionInfo
    {
        public string MetaWeblogURL { get; set; }
        public string BlogID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static BlogConnectionInfo Load(string filename)
        {
            var doc = System.Xml.Linq.XDocument.Load(filename);
            var root = doc.Root;

            string blogId = root.GetElementString("blogid");
            string metaWeblogUrl = root.GetElementString("metaweblog_url");
            string username = root.GetElementString("username");
            string password = root.GetElementString("password");

            var coninfo = new BlogConnectionInfo(metaWeblogUrl, blogId, username, password);

            return coninfo;
        }
        
        public BlogConnectionInfo(string metaweblogurl, string blogid, string username, string password)
        {
            this.BlogID = blogid;
            this.MetaWeblogURL = metaweblogurl;
            this.Username = username;
            this.Password = password;
        }
    }
}