namespace MetaWeblogSharp
{
    public class BlogAccount
    {
        public string BlogID { get; set; }
        public string MetaWeblogURL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static BlogAccount Load(string filename)
        {
            var doc = System.Xml.Linq.XDocument.Load(filename);
            var root = doc.Root;
            var bi = new BlogAccount();
            
            bi.BlogID = root.Element("blogid").Value;
            bi.MetaWeblogURL = root.Element("metaweblog_url").Value;
            bi.Username = root.Element("username").Value;
            bi.Password = root.Element("password").Value;

            return bi;
        }


        public BlogAccount()
        {
        }

        public BlogAccount(string blogid, string metaweblogurl, string username, string password)
        {
            this.BlogID = blogid;
            this.MetaWeblogURL = metaweblogurl;
            this.Username = username;
            this.Password = password;
        }
    }
}