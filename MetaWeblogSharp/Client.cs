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
            var array = (XmlRPC.Array) param;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new PostInfo();
                pi.Title = (string)struct_["title"];
                pi.DateCreated = (System.DateTime) struct_["dateCreated"];
                pi.Link = (string)struct_["link"];
                pi.PostID = (string)struct_["postid"];
                pi.UserID = struct_.GetString("userid");
                pi.CommentCount = struct_.GetInt("commentCount");
                pi.PostStatus = struct_.GetString("post_status");
                pi.PermaLink = struct_.GetString("permaLink");
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
            item.PostID = struct_.GetString("postid");
            item.Description = struct_.GetString("description");
            //item.Tags
            item.Link = struct_.GetString("link");
            item.DateCreated = (System.DateTime)struct_["dateCreated"];
            item.PermaLink = struct_.GetString("permaLink");
            item.PostStatus = struct_.GetString("post_status");
            item.Title = struct_.GetString("title");
            item.UserID = struct_.GetString("userid");

            item.RawData = struct_;
            return item;
        }

        public string NewPost(string title, string description, IList<string> categories, bool publish)
        {
            XmlRPC.Array cats=null;

            if (categories == null)
            {
                cats = new XmlRPC.Array(0);
            }
            else
            {
                cats = new XmlRPC.Array(categories.Count);
                cats.AddRange(categories.Cast<object>());
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
            var list = (XmlRPC.Array)response.Parameters[0];
            var struct_ = (XmlRPC.Struct)list[0];
            var item = new BlogInfo();

            //item.Categories 
            item.BlogID= (string)struct_["blogid"];
            item.URL= (string)struct_["url"];
            item.BlogName= (string)struct_["blogName"];
            item.IsAdmin = struct_.GetBool("isAdmin");
            item.SiteName = struct_.GetString("siteName");
            item.Capabilities = struct_.GetString("capabilities");
            item.XmlRPCEndPoint = struct_.GetString("xmlrpc");
            item.RawData = struct_;
            return item;
        }


        public bool EditPost(string postid, string title, string description, IList<string> categories, bool publish)
        {
            // Create an array to hold any categories
            XmlRPC.Array categories_;
            if (categories == null)
            {
                categories_ = new XmlRPC.Array(0);
            }

            categories_ = new XmlRPC.Array(categories.Count);
            categories_.AddRange(categories.Cast<object>());
            
            var service = new XmlRPC.Service(this.URL);
            var struct_ = new XmlRPC.Struct();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = categories_;

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
            var array = (XmlRPC.Array)param;

            var items = new List<CategoryInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new CategoryInfo();
                pi.Title = struct_.GetString("title");
                pi.Description = struct_.GetString("description");
                pi.HTMLURL = struct_.GetString("htmlUrl");
                pi.RSSURL = struct_.GetString("rssUrl");
                pi.CategoryID = struct_.GetString("categoryid");

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

            item.UserID = struct_.GetString("userid");
            item.Nickname = struct_.GetString("nickname");
            item.FirstName = struct_.GetString("firstname");
            item.LastName = struct_.GetString("lastname");
            item.URL = struct_.GetString("url");

            item.RawData = struct_;
            return item;
        }
    }
}