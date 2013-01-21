using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp
{
    public class Client
    {
        //http://xmlrpc.scripting.com/metaWeblogApi.html
        
        public string AppKey = "0123456789ABCDEF";
        public BlogAccount BlogAccount;

        public Client(BlogAccount account)
        {
            this.BlogAccount = account;
        }

        public List<PostInfo> GetRecentPosts(int numposts)
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getRecentPosts");
            method.AddParameter(this.BlogAccount.BlogID);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);
            method.AddParameter(numposts);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (XmlRPC.Array) param;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new PostInfo();
                pi.Title = struct_.GetItem<string>("title",null);
                pi.DateCreated = struct_.GetItem<System.DateTime>("dateCreated",System.DateTime.MaxValue);
                pi.Link = struct_.GetItem<string>("link",null);
                pi.PostID = struct_.GetItem<string>("postid", null);
                pi.UserID = struct_.GetItem<string>("userid", null);
                pi.CommentCount = struct_.GetItem<int>("commentCount",0);
                pi.PostStatus = struct_.GetItem<string>("post_status",null);
                pi.PermaLink = struct_.GetItem<string>("permaLink",null);
                pi.Description = struct_.GetItem<string>("description", null);
                pi.RawData = struct_;

                items.Add(pi);
            }
            return items;
        }

        public MediaObjectInfo NewMediaObject(string name, string type, byte [] bits)
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var input_struct_ = new XmlRPC.Struct();
            input_struct_["name"] = name;
            input_struct_["type"] = type;
            input_struct_["bits"] = bits;

            var method = new XmlRPC.MethodCall("metaWeblog.newMediaObject");
            method.AddParameter(this.BlogAccount.BlogID);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);
            method.AddParameter(input_struct_);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new MediaObjectInfo();

            item.URL = struct_.GetItem<string>("url",null);
            item.RawData = struct_;

            return item;
        }

        public PostInfo GetPost(string postid)
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getPost");
            method.AddParameter(postid); // notice this is the postid, not the blogid
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new PostInfo();

            //item.Categories 
            item.PostID = struct_.GetItem<string>("postid",null);
            item.Description = struct_.GetItem<string>("description", null);
            //item.Tags
            item.Link = struct_.GetItem<string>("link", null);
            item.DateCreated = struct_.GetItem<System.DateTime>("dateCreated",System.DateTime.MaxValue);
            item.PermaLink = struct_.GetItem<string>("permaLink", null);
            item.PostStatus = struct_.GetItem<string>("post_status", null);
            item.Title = struct_.GetItem<string>("title", null);
            item.UserID = struct_.GetItem<string>("userid", null);

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

            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var struct_ = new XmlRPC.Struct();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = cats;

            var method = new XmlRPC.MethodCall("metaWeblog.newPost");
            method.AddParameter(this.BlogAccount.BlogID);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var postid = (string) param;

            return postid;
        }

        public bool DeletePost(string postid)
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.deletePost");
            method.AddParameter(AppKey);
            method.AddParameter(postid);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);
            method.AddParameter(true);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success= (bool)param;

            return success;
        }

        public BlogInfo GetUsersBlogs()
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.getUsersBlogs");
            method.AddParameter(this.AppKey);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);

            var response = service.Execute(method);
            var list = (XmlRPC.Array)response.Parameters[0];
            var struct_ = (XmlRPC.Struct)list[0];
            var item = new BlogInfo();

            //item.Categories 
            item.BlogID= struct_.GetItem<string>("blogid",null);
            item.URL= struct_.GetItem<string>("url",null);
            item.BlogName = struct_.GetItem<string>("blogName", null);
            item.IsAdmin = struct_.GetItem<bool>("isAdmin",false);
            item.SiteName = struct_.GetItem<string>("siteName", null);
            item.Capabilities = struct_.GetItem<string>("capabilities", null);
            item.XmlRPCEndPoint = struct_.GetItem<string>("xmlrpc", null);
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

            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);
            var struct_ = new XmlRPC.Struct();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = categories_;

            var method = new XmlRPC.MethodCall("metaWeblog.editPost");
            method.AddParameter(postid);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success = (bool)param;

            return success;
        }

        public List<CategoryInfo> GetCategories()
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getCategories");
            method.AddParameter(this.BlogAccount.BlogID);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (XmlRPC.Array)param;

            var items = new List<CategoryInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var pi = new CategoryInfo();
                pi.Title = struct_.GetItem<string>("title", null);
                pi.Description = struct_.GetItem<string>("description", null);
                pi.HTMLURL = struct_.GetItem<string>("htmlUrl", null);
                pi.RSSURL = struct_.GetItem<string>("rssUrl", null);
                pi.CategoryID = struct_.GetItem<string>("categoryid", null);

                pi.RawData = struct_;
                items.Add(pi);
            }
            return items;
        }

        public UserInfo GetUserInfo()
        {
            var service = new XmlRPC.Service(this.BlogAccount.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.getUserInfo");
            method.AddParameter(this.AppKey);
            method.AddParameter(this.BlogAccount.Username);
            method.AddParameter(this.BlogAccount.Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            var item = new UserInfo();

            item.UserID = struct_.GetItem<string>("userid", null);
            item.Nickname = struct_.GetItem<string>("nickname", null);
            item.FirstName = struct_.GetItem<string>("firstname", null);
            item.LastName = struct_.GetItem<string>("lastname", null);
            item.URL = struct_.GetItem<string>("url", null);

            item.RawData = struct_;
            return item;
        }
    }
}