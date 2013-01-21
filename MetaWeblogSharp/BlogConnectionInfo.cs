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
            
            string _BlogID = root.Element("blogid").Value;
            string _MetaWeblogURL = root.Element("metaweblog_url").Value;
            string _Username = root.Element("username").Value;
            string _Password = root.Element("password").Value;

            var bi = new BlogConnectionInfo(_MetaWeblogURL, _BlogID, _Username, _Password);

            return bi;
        }
        
        private BlogConnectionInfo()
        {
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