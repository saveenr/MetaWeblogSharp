using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            // string blogdef = "\\saveenr\\skydrive\\private\\reference\\msdn.xml";
            string blogdef = "\\saveenr\\skydrive\\private\\reference\\viziblr.xml";

            var blogcon = MetaWeblogSharp.BlogConnectionInfo.Load(blogdef);
            var client = new MetaWeblogSharp.Client(blogcon);

            var posts = client.GetRecentPosts(10);

            var first_post_1 = posts[0];
            var first_post_2 = client.GetPost(first_post_1.PostID);
            var bytes = System.IO.File.ReadAllBytes("test1.png");

            var mo = client.NewMediaObject("8901234567890.png", "image/png", bytes);

            var categories1 = new List<string>
            {
                // "A", "B", "C"
            };

            var categories2 = client.GetCategories();

            var blogs = client.GetUsersBlogs();

            var title1 = string.Format("Test {0}" , System.DateTime.Now.ToString());
            var description1 = string.Format("<p>Hello World</p>\n<p>{0}</p>", System.DateTime.Now);
            var new_post_id = client.NewPost(title1, description1, categories1, false);
            var new_post_info = client.GetPost(new_post_id);
            var successedit1 = client.EditPost(new_post_info.PostID, title1, description1, categories1, false);
            var new_post_info2 = client.GetPost(new_post_id);
            var success_delete = client.DeletePost(new_post_id);

            try
            {
                // this will raise an exception if the post does not exist
                var x = client.GetPost(new_post_id);
            }
            catch (MetaWeblogSharp.XmlRPC.XmlRPCException)
            {
                // do nothing
            }

            var u1 = client.GetUserInfo();
        }
    }
}
