using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp
{
    public class Client
    {
        //http://xmlrpc.scripting.com/metaWeblogApi.html
        
        public string URL;
        public string BlogID;
        public string Username;
        public string Password;
        public string AppKey = "0123456789ABCDEF";

        public Client(string url, string blogid, string user, string password)
        {
            this.URL = url;
            this.BlogID = blogid;
            this.Username = user;
            this.Password = password;
        }

        public List<PostInfo> GetRecentPosts(int numposts)
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.getRecentPosts");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(numposts);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (List<object>) param;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new PostInfo();
                pi.Title = (string)struct_["title"];
                pi.DateCreated = (System.DateTime) struct_["dateCreated"];
                pi.Link = (string)struct_["link"];
                pi.PostID = (string)struct_["postid"];
                pi.UserID = getstring(struct_,"userid");
                pi.CommentCount = getint(struct_,"commentCount");
                pi.PostStatus = getstring(struct_,"post_status");
                pi.PermaLink= getstring(struct_,"permaLink");
                pi.Description = (string)struct_["description"];
                pi.RawData = struct_;

                items.Add(pi);
            }
            return items;
        }

        public MediaObjectInfo NewMediaObject(string name, string type, byte [] bits)
        {
            var service = new XmlRPC.Service(this.URL);

            var input_struct_ = new XmlRPC.Struct();
            input_struct_["name"] = name;
            input_struct_["type"] = type;
            input_struct_["bits"] = bits;

            var method = new XmlRPC.MethodCall("metaWeblog.newMediaObject");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(input_struct_);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new MediaObjectInfo();

            item.URL = (string) struct_["url"];
            item.RawData = struct_;

            return item;
        }

        public PostInfo GetPost(string postid)
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.getPost");
            method.AddParameter(postid); // notice this is the postid, not the blogid
            method.AddParameter(Username);
            method.AddParameter(Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new PostInfo();

            //item.Categories 
            item.PostID = (string) struct_["postid"];
            item.Description = (string) struct_["description"];
            //item.Tags
            item.Link = (string)struct_["link"];
            item.DateCreated = (System.DateTime)struct_["dateCreated"];
            item.PermaLink = getstring(struct_,"permaLink");
            item.PostStatus = getstring(struct_,"post_status");
            item.Title = (string)struct_["title"];
            item.UserID = getstring(struct_,"userid");

            item.RawData = struct_;
            return item;
        }

        private string getstring(IDictionary<string, object> dic, string name)
        {
            if (dic.ContainsKey(name))
            {
                return (string)dic[name];
            }
            else
            {
                return null;
            }
        }

        private int getint(IDictionary<string, object> dic, string name)
        {
            if (dic.ContainsKey(name))
            {
                return (int)dic[name];
            }
            else
            {
                return 0;
            }
        }

        private bool getbool(IDictionary<string, object> dic, string name)
        {
            if (dic.ContainsKey(name))
            {
                return (bool)dic[name];
            }
            else
            {
                return false;
            }
        }

        public string NewPost(string title, string description, IList<string> categories, bool publish)
        {
            List<object> cats=null;

            if (categories == null)
            {
                cats = new List<object>(0);
            }
            else
            {
                cats = categories.Select(i => (object) i).ToList();
            }

            var service = new XmlRPC.Service(this.URL);

            var struct_ = new XmlRPC.Struct();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = cats;

            var method = new XmlRPC.MethodCall("metaWeblog.newPost");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var postid = (string) param;

            return postid;
        }

        public bool DeletePost(string postid)
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("blogger.deletePost");
            method.AddParameter(AppKey);
            method.AddParameter(postid);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(true);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success= (bool)param;

            return success;
        }

        public BlogInfo GetUsersBlogs()
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.getUsersBlogs");
            method.AddParameter(this.AppKey); 
            method.AddParameter(Username);
            method.AddParameter(Password);

            var response = service.Execute(method);
            var list = (List<object>)response.Parameters[0];
            var struct_ = (XmlRPC.Struct)list[0];
            var item = new BlogInfo();

            //item.Categories 
            item.BlogID= (string)struct_["blogid"];
            item.URL= (string)struct_["url"];
            item.BlogName= (string)struct_["blogName"];
            item.IsAdmin = getbool(struct_,"isAdmin");
            item.SiteName= getstring(struct_,"siteName");
            item.Capabilities = getstring(struct_,"capabilities");
            item.XmlRPCEndPoint = getstring(struct_, "xmlrpc");
            item.RawData = struct_;
            return item;
        }



        private static List<object> create_cats(IList<string> categories)
        {
            if (categories == null)
            {
                return new List<object>(0);
            }

            var cats = new List<object>(categories.Count);
            cats = categories.Select(i => (object)i).ToList();
            return cats;
        }


                public bool EditPost(string postid, string title, string description, IList<string> categories, bool publish)
        {
            var cats = create_cats(categories);

            var service = new XmlRPC.Service(this.URL);

            var struct_ = new XmlRPC.Struct();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = cats;

            var method = new XmlRPC.MethodCall("metaWeblog.editPost");
            method.AddParameter(postid);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success = (bool)param;

            return success;
        }

        public List<CategoryInfo> GetCategories()
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.getCategories");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (List<object>)param;

            var items = new List<CategoryInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new CategoryInfo();
                pi.Title = getstring(struct_,"title");
                pi.Description = getstring(struct_, "description");
                pi.HTMLURL= getstring(struct_, "htmlUrl");
                pi.RSSURL= getstring(struct_, "rssUrl");
                pi.CategoryID= getstring(struct_, "categoryid");

                pi.RawData = struct_;
                items.Add(pi);
            }
            return items;
        }

        public UserInfo GetUserInfo()
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("blogger.getUserInfo");
            method.AddParameter(this.AppKey);
            method.AddParameter(Username);
            method.AddParameter(Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new UserInfo();

            item.UserID = getstring(struct_, "userid");
            item.Nickname= getstring(struct_, "nickname");
            item.FirstName= getstring(struct_, "firstname");
            item.LastName= getstring(struct_, "lastname");
            item.URL= getstring(struct_, "url");

            item.RawData = struct_;
            return item;
        }

    }

    public class UserInfo
    {
        public string UserID;
        public string Nickname;
        public string FirstName;
        public string LastName;
        public string URL;

        public object RawData;
    }

    public class CategoryInfo
    {
        public string Description;
        public string HTMLURL;
        public string RSSURL;
        public string Title;
        public string CategoryID;

        public object RawData;
    }
}