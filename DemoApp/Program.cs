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
            //var doc = System.Xml.Linq.XDocument.Load("D:\\saveenr\\skydrive\\msdn.xml");
            var doc = System.Xml.Linq.XDocument.Load("D:\\saveenr\\skydrive\\viziblr.xml");

            var root = doc.Root;
            string blogid = root.Element("blogid").Value;
            string blog_metweblog_url = root.Element("metaweblog_url").Value;
            string username = root.Element("username").Value;
            string password = root.Element("password").Value;


            var service = new MetaWeblogSharp.Service(blog_metweblog_url, blogid, username, password);
            var response = service.GetRecentPosts(10);

            var first_post_1 = response[0];
            var first_post_2 = service.GetPost(first_post_1.PostID);
            var bytes = System.IO.File.ReadAllBytes("test1.png");

            var response2 = service.NewMediaObject("12356789012356/789012356789012345678901234567890.png", "image/png", bytes);

            var categories1 = new List<string>
            {
                // "A", "B", "C"
            };

            var blogs = service.GetUsersBlogs();

            var title1 = "test" + System.DateTime.Now.ToString();
            var description1 = string.Format("<p>HI {0}</p>", System.DateTime.Now);
            var new_post_id = service.NewPost(title1, description1, categories1, false);
            var new_post_info = service.GetPost(new_post_id);

            var successedit1 = service.EditPost(new_post_info.PostID, title1 + "XXX", description1 + "XXX", categories1, false);

            var new_post_info2 = service.GetPost(new_post_id);

            var success_delete = service.DeletePost(new_post_id);

        }
    }
}
