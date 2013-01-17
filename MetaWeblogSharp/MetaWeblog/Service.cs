using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp
{
    public class Service
    {
        //http://xmlrpc.scripting.com/metaWeblogApi.html


        public string URL;
        public string BlogID;
        public string Username;
        public string Password;

        public Service(string url, string blogid, string user, string password)
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

            var response = service.ExecuteRaw(method);

            var param = response.Parameters[0];
            var array = (List<object>) param;

            var items = new List<PostInfo>();
            foreach (var value in array)
            {
                var struct_ = (Dictionary<string,object>)value;

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
                items.Add(pi);
                int x = 1;
            }
            return items;
        }

        public MediaObjectInfo NewMediaObject(string name, string type, byte [] bits)
        {
            var service = new XmlRPC.Service(this.URL);

            var input_struct_ = new Dictionary<string, object>();
            input_struct_["name"] = name;
            input_struct_["type"] = type;
            input_struct_["bits"] = bits;

            var method = new XmlRPC.MethodCall("metaWeblog.newMediaObject");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(input_struct_);

            var response = service.ExecuteRaw(method);
            var param = response.Parameters[0];
            var struct_ = (Dictionary<string,object>) param;
            var item = new MediaObjectInfo();

            item.URL = (string) struct_["url"];

            return item;
        }

        public PostInfo GetPost(string postid)
        {
            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.getPost");
            method.AddParameter(postid); // notice this is the postid, not the blogid
            method.AddParameter(Username);
            method.AddParameter(Password);

            var response = service.ExecuteRaw(method);
            var param = response.Parameters[0];
            var struct_ = (Dictionary<string, object>)param;
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

            var struct_ = new Dictionary<string, object>();
            struct_["title"] = title;
            struct_["description"] = description;
            struct_["categories"] = cats;

            var method = new XmlRPC.MethodCall("metaWeblog.newPost");
            method.AddParameter(BlogID);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(struct_);
            method.AddParameter(publish);

            var response = service.ExecuteRaw(method);
            var param = response.Parameters[0];
            var postid = (string) param;

            return postid;
        }

        public bool DeletePost(string postid)
        {
            string appkey = "0123456789ABCDEF";

            var service = new XmlRPC.Service(this.URL);

            var method = new XmlRPC.MethodCall("metaWeblog.deletePost");
            method.AddParameter(appkey);
            method.AddParameter(postid);
            method.AddParameter(Username);
            method.AddParameter(Password);
            method.AddParameter(true);

            var response = service.ExecuteRaw(method);
            var param = response.Parameters[0];
            var success= (bool)param;

            return success;
        }

    }
}