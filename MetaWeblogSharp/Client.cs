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
                pi.Title = struct_.GetString("title",null).Data;
                pi.DateCreated = struct_.GetItem<DateTimeX>("dateCreated",null).Data;
                pi.Link = struct_.GetString("link",null).Data;
                pi.PostID = struct_.GetString("postid", null).Data;
                pi.UserID = struct_.GetString("userid", null).Data;
                pi.CommentCount = struct_.GetItem<IntegerX>("commentCount").Data;
                pi.PostStatus = struct_.GetString("post_status", null).Data;
                pi.PermaLink = struct_.GetString("permaLink", null).Data;
                pi.Description = struct_.GetString("description", null).Data;
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

            item.URL = struct_.GetString("url", null).Data;
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
            item.PostID = struct_.GetString("postid", null).Data;
            item.Description = struct_.GetString("description", null).Data;
            //item.Tags
            item.Link = struct_.GetString("link", null).Data;
            item.DateCreated = struct_.GetItem<DateTimeX>("dateCreated",null).Data;
            item.PermaLink = struct_.GetString("permaLink", null).Data;
            item.PostStatus = struct_.GetString("post_status", null).Data;
            item.Title = struct_.GetString("title", null).Data;
            item.UserID = struct_.GetString("userid", null).Data;

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
            var postid = ((StringX)param.Data).Data;

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
            var success = (BooleanX)param.Data;

            return success.Data;
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
            item.BlogID = struct_.GetString("blogid", null).Data;
            item.URL = struct_.GetString("url", null).Data;
            item.BlogName = struct_.GetString("blogName", null).Data;
            item.IsAdmin = struct_.GetItem<bool>("isAdmin",false);
            item.SiteName = struct_.GetString("siteName", null).Data;
            item.Capabilities = struct_.GetString("capabilities", null).Data;
            item.XmlRPCEndPoint = struct_.GetString("xmlrpc", null).Data;
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
            var success = (BooleanX)param.Data;

            return success.Data;
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
                pi.Title = struct_.GetString("title", null).Data;
                pi.Description = struct_.GetString("description", null).Data;
                pi.HTMLURL = struct_.GetString("htmlUrl", null).Data;
                pi.RSSURL = struct_.GetString("rssUrl", null).Data;
                pi.CategoryID = struct_.GetString("categoryid", null).Data;

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

            item.UserID = struct_.GetString("userid", null).Data;
            item.Nickname = struct_.GetString("nickname", null).Data;
            item.FirstName = struct_.GetString("firstname", null).Data;
            item.LastName = struct_.GetString("lastname", null).Data;
            item.URL = struct_.GetString("url", null).Data;

            item.RawData = struct_;
            return item;
        }
    }
}