namespace MetaWeblogSharp
{
    public class BlogAccount
    {
        public string BlogID;
        public string MetaWeblogURL;
        public string Username;
        public string Password;

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
    }
}