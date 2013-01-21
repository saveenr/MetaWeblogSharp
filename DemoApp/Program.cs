using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // string blogdef = "D:\\saveenr\\skydrive\\msdn.xml";
            string blogdef = "D:\\saveenr\\skydrive\\viziblr.xml";

            var blogaccount = MetaWeblogSharp.BlogAccount.Load(blogdef);

            var service = new MetaWeblogSharp.Client(blogaccount);
            var posts = service.GetRecentPosts(10);

            var first_post_1 = posts[0];
            var first_post_2 = service.GetPost(first_post_1.PostID);
            var bytes = System.IO.File.ReadAllBytes("test1.png");

            var response2 = service.NewMediaObject("8901234567890.png", "image/png", bytes);

            var categories1 = new List<string>
            {
                // "A", "B", "C"
            };

            var categories2 = service.GetCategories();

            var blogs = service.GetUsersBlogs();

            var title1 = "test" + System.DateTime.Now.ToString();
            var description1 = string.Format("<p>HI {0}</p>", System.DateTime.Now);
            var new_post_id = service.NewPost(title1, description1, categories1, false);
            var new_post_info = service.GetPost(new_post_id);

            var successedit1 = service.EditPost(new_post_info.PostID, title1 + "XXX", description1 + "XXX", categories1, false);

            var new_post_info2 = service.GetPost(new_post_id);

            var success_delete = service.DeletePost(new_post_id);

            try
            {
                // this will raise an exception if the post does not exist
                var x = service.GetPost(new_post_id);
            }
            catch (MetaWeblogSharp.XmlRPC.XmlRPCException exc)
            {
                // do nothing
            }

            var u1 = service.GetUserInfo();
        }
    }
}
