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

            string blogId = get_element_string(root,"blogid");
            string metaWeblogUrl = get_element_string(root,"metaweblog_url");
            string username = get_element_string(root,"username");
            string password = get_element_string(root,"password");

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

        private static string get_element_string(System.Xml.Linq.XElement parent, string name)
        {
            var child_el = parent.Element(name);
            if (child_el == null)
            {
                string msg = string.Format("Error while while loading {0} class. Xml is missing {0} element",
                                           typeof (BlogConnectionInfo).Name, name);
                throw new MetaWeblogException(msg);
            }

            return child_el.Value;
        }
    }
}