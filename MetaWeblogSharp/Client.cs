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
            var array = (XmlRPC.Array)param;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var postinfo = new PostInfo();
                postinfo.Title = struct_.Get<StringValue>("title", StringValue.NullString).String;
                postinfo.DateCreated = struct_.Get<DateTimeValue>("dateCreated").Data;
                postinfo.Link = struct_.Get<StringValue>("link", StringValue.NullString).String;
                postinfo.PostID = struct_.Get<StringValue>("postid", StringValue.NullString).String;
                postinfo.UserID = struct_.Get<StringValue>("userid", StringValue.NullString).String;
                postinfo.CommentCount = struct_.Get<IntegerValue>("commentCount").Integer;
                postinfo.PostStatus = struct_.Get<StringValue>("post_status", StringValue.NullString).String;
                postinfo.PermaLink = struct_.Get<StringValue>("permaLink", StringValue.NullString).String;
                postinfo.Description = struct_.Get<StringValue>("description", StringValue.NullString).String;
                postinfo.RawData = struct_;

                items.Add(postinfo);
            }
            return items;
        }

        public MediaObjectInfo NewMediaObject(string name, string type, byte [] bits)
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var input_struct_ = new XmlRPC.Struct();
            input_struct_["name"] = new StringValue(name);
            input_struct_["type"] = new StringValue(type);
            input_struct_["bits"] = new Base64Data(bits);

            var method = new XmlRPC.MethodCall("metaWeblog.newMediaObject");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(input_struct_);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var struct_ = (XmlRPC.Struct)param;
            
            var mediaobject = new MediaObjectInfo();
            mediaobject.URL = struct_.Get<StringValue>("url", StringValue.NullString).String;
            mediaobject.RawData = struct_;

            return mediaobject;
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
            var struct_ = (XmlRPC.Struct)param;

            var postinfo = new PostInfo();
            //item.Categories 
            postinfo.PostID = struct_.Get<StringValue>("postid").String;
            postinfo.Description = struct_.Get<StringValue>("description").String;
            //item.Tags
            postinfo.Link = struct_.Get<StringValue>("link", StringValue.NullString).String;
            postinfo.DateCreated = struct_.Get<DateTimeValue>("dateCreated").Data;
            postinfo.PermaLink = struct_.Get<StringValue>("permaLink", StringValue.NullString).String;
            postinfo.PostStatus = struct_.Get<StringValue>("post_status", StringValue.NullString).String;
            postinfo.Title = struct_.Get<StringValue>("title").String;
            postinfo.UserID = struct_.Get<StringValue>("userid", StringValue.NullString).String;

            postinfo.RawData = struct_;
            return postinfo;
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
                cats.AddRange( categories.Select(c=>new StringValue(c)));
            }

            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var struct_ = new XmlRPC.Struct();
            struct_["title"] = new StringValue(title);
            struct_["description"] = new StringValue(description);
            struct_["categories"] = cats;

            var method = new XmlRPC.MethodCall("metaWeblog.newPost");
            method.AddParameter(this.BlogConnectionInfo.BlogID);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var postid = ((StringValue)param).String;

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
            var success = (BooleanValue)param;

            return success.Boolean;
        }

        public BlogInfo GetUsersBlogs()
        {
            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);

            var method = new XmlRPC.MethodCall("blogger.getUsersBlogs");
            method.AddParameter(this.AppKey);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);

            var response = service.Execute(method);
            var list = (XmlRPC.Array)response.Parameters[0];
            var struct_ = (XmlRPC.Struct)list[0];

            var boginfo = new BlogInfo();
            boginfo.BlogID = struct_.Get<StringValue>("blogid", StringValue.NullString).String;
            boginfo.URL = struct_.Get<StringValue>("url", StringValue.NullString).String;
            boginfo.BlogName = struct_.Get<StringValue>("blogName", StringValue.NullString).String;
            boginfo.IsAdmin = struct_.Get<BooleanValue>("isAdmin", false).Boolean;
            boginfo.SiteName = struct_.Get<StringValue>("siteName", StringValue.NullString).String;
            boginfo.Capabilities = struct_.Get<StringValue>("capabilities", StringValue.NullString).String;
            boginfo.XmlRPCEndPoint = struct_.Get<StringValue>("xmlrpc", StringValue.NullString).String;
            boginfo.RawData = struct_;
            
            return boginfo;
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
            categories_.AddRange(categories.Select(c=>new StringValue(c)));

            var service = new XmlRPC.Service(this.BlogConnectionInfo.MetaWeblogURL);
            var struct_ = new XmlRPC.Struct();
            struct_["title"] = new StringValue(title);
            struct_["description"] = new StringValue(description);
            struct_["categories"] = categories_;

            var method = new XmlRPC.MethodCall("metaWeblog.editPost");
            method.AddParameter(postid);
            method.AddParameter(this.BlogConnectionInfo.Username);
            method.AddParameter(this.BlogConnectionInfo.Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.Execute(method);
            var param = response.Parameters[0];
            var success = (BooleanValue)param;

            return success.Boolean;
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
            var array = (XmlRPC.Array)param;

            var items = new List<CategoryInfo>();
            foreach (var value in array)
            {
                var struct_ = (XmlRPC.Struct)value;

                var catinfo = new CategoryInfo();
                catinfo.Title = struct_.Get<StringValue>("title", StringValue.NullString).String;
                catinfo.Description = struct_.Get<StringValue>("description", StringValue.NullString).String;
                catinfo.HTMLURL = struct_.Get<StringValue>("htmlUrl", StringValue.NullString).String;
                catinfo.RSSURL = struct_.Get<StringValue>("rssUrl", StringValue.NullString).String;
                catinfo.CategoryID = struct_.Get<StringValue>("categoryid", StringValue.NullString).String;

                catinfo.RawData = struct_;
                items.Add(catinfo);
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
            var struct_ = (XmlRPC.Struct)param;
            var item = new UserInfo();

            item.UserID = struct_.Get<StringValue>("userid", StringValue.NullString).String;
            item.Nickname = struct_.Get<StringValue>("nickname", StringValue.NullString).String;
            item.FirstName = struct_.Get<StringValue>("firstname", StringValue.NullString).String;
            item.LastName = struct_.Get<StringValue>("lastname", StringValue.NullString).String;
            item.URL = struct_.Get<StringValue>("url", StringValue.NullString).String;

            item.RawData = struct_;
            return item;
        }
    }
}