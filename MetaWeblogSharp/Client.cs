using System.Collections.Generic;
using System.Linq;
using MetaWeblogSharp.XmlRPC;

namespace MetaWeblogSharp
{
    public class Client
    {
        //http://xmlrpc.scripting.com/metaWeblogApi.html
        
        public string AppKey = "0123456789ABCDEF";
        public BlogConnectionInfo BlogConnectionInfo;

        public Client(BlogConnectionInfo connectionInfo)
        {
            this.BlogConnectionInfo = connectionInfo;
        }

        public List<PostInfo> GetRecentPosts(int numposts)
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getRecentPosts");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(numposts);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (XmlRPC.Array)param.Data;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value.Data;

                var pi = new PostInfo();
                pi.Title = struct_.GetItem<string>("title",null);
                pi.DateCreated = struct_.GetItem<DateTimeX>("dateCreated",null).Data;
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
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var input_struct_ = new XmlRPC.Struct();
            input_struct_["name"] = new XmlRPC.Value(name);
            input_struct_["type"] = new XmlRPC.Value(type);
            input_struct_["bits"] = new XmlRPC.Value( new Base64Data(bits));

            var method = new XmlRPC.MethodCall("metaWeblog.newMediaObject");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(input_struct_);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param.Data;
            var item = new MediaObjectInfo();

            item.URL = struct_.GetItem<string>("url",null);
            item.RawData = struct_;

            return item;
        }

        public PostInfo GetPost(string postid)
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getPost");
            method.AddParameter(postid); // notice this is the postid, not the blogid
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param.Data;
            var item = new PostInfo();

            //item.Categories 
            item.PostID = struct_.GetItem<string>("postid",null);
            item.Description = struct_.GetItem<string>("description", null);
            //item.Tags
            item.Link = struct_.GetItem<string>("link", null);
            item.DateCreated = struct_.GetItem<DateTimeX>("dateCreated",null).Data;
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
                cats.AddRange( categories.Select(c=>new Value(c)));
            }

            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var struct_ = new XmlRPC.Struct();
            struct_["title"] = new XmlRPC.Value(title);
            struct_["description"] = new XmlRPC.Value(description);
            struct_["categories"] = new XmlRPC.Value(cats);

            var method = new XmlRPC.MethodCall("metaWeblog.newPost");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var postid = (string)param.Data;

            return postid;
        }

        public bool DeletePost(string postid)
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.deletePost");
            method.AddParameter(AppKey);
            method.AddParameter(postid);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(true);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success = (bool)param.Data;

            return success;
        }

        public BlogInfo GetUsersBlogs()
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.getUsersBlogs");
            method.AddParameter(this.AppKey);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);

            var response = service.Execute(method);
            var list = (XmlRPC.Array)response.Parameters[0].Data;
            var struct_ = (XmlRPC.Struct)list[0].Data;
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
            categories_.AddRange(categories.Select(c=>new Value(c)));

            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);
            var struct_ = new XmlRPC.Struct();
            struct_["title"] = new XmlRPC.Value(title);
            struct_["description"] = new XmlRPC.Value(description);
            struct_["categories"] = new XmlRPC.Value(categories_);

            var method = new XmlRPC.MethodCall("metaWeblog.editPost");
            method.AddParameter(postid);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success = (bool)param.Data;

            return success;
        }

        public List<CategoryInfo> GetCategories()
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("metaWeblog.getCategories");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);

            var response = service.Execute(method);

            var param = response.Parameters[0];
            var array = (XmlRPC.Array)param.Data;

            var items = new List<CategoryInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value.Data;

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
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.getUserInfo");
            method.AddParameter(this.AppKey);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param.Data;
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