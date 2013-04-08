using System;
using System.Xml.Serialization;
using MetaWeblogSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaWeblogSharp.XmlRPC;
using X=MetaWeblogSharp.XmlRPC;

namespace MetaWeblogSharpTests
{
    [TestClass]
    public class Test_BlogEngine
    {
        [TestMethod]
        public void GetPosts1()
        {

            var con1 = GetBlog1();

            var con2 = new MetaWeblogSharp.BlogConnectionInfo(
                "http://localhost:14228/test2",
                "http://localhost:14882/test2/metaweblog.axd",
                "test2",
                "admin",
                "admin");


            var client = new MetaWeblogSharp.Client(con1);

      

            var posts = client.GetRecentPosts(10000);
            foreach (var p in posts)
            {
                client.DeletePost(p.PostID);
            }

            posts = client.GetRecentPosts(10000);
            Assert.AreEqual(0,posts.Count);

            // create and verify a normal post
            string postid = client.NewPost("P1", "P1Content", null, false, null);
            posts = client.GetRecentPosts(10000);
            Assert.AreEqual(1,posts.Count);
            Assert.AreEqual(postid, posts[0].PostID);


            // Create another post
            string postid2 = client.NewPost("P2", "P2Content", null, false, null);
            posts = client.GetRecentPosts(10000);
            Assert.AreEqual(2, posts.Count);
            Assert.AreEqual(postid2, posts[0].PostID);
            Assert.AreEqual(postid, posts[1].PostID);
            Assert.AreEqual(null, posts[0].PostStatus);


            var first_post = posts[0];
            string new_title = first_post.Title + " Updated";
            client.EditPost(first_post.PostID, new_title , first_post.Description, null, true);
            var first_post_updated = client.GetPost(first_post.PostID);
            Assert.AreEqual(new_title, first_post_updated.Title);



        }

        private static BlogConnectionInfo GetBlog1()
        {
            var con1 = new MetaWeblogSharp.BlogConnectionInfo(
                "http://localhost:14228/test1",
                "http://localhost:14882/test1/metaweblog.axd",
                "test1",
                "admin",
                "admin");
            return con1;
        }

        private static BlogConnectionInfo GetBlog2()
        {
            var con1 = new MetaWeblogSharp.BlogConnectionInfo(
                "http://localhost:14228/test2",
                "http://localhost:14882/test2/metaweblog.axd",
                "test2",
                "admin",
                "admin");
            return con1;
        }

        [TestMethod]
        public void SerializeTests()
        {
            var con1 = new MetaWeblogSharp.BlogConnectionInfo(
                "http://localhost:14228/test1",
                "http://localhost:14882/test1/metaweblog.axd",
                "test1",
                "admin",
                "admin");


             var client1 = new MetaWeblogSharp.Client(con1);
             var posts = client1.GetRecentPosts(100000).ToArray();

             MetaWeblogSharp.PostInfo.Serialize(posts,@"d:\blog_con_info.xml");

             var loaded_posts = MetaWeblogSharp.PostInfo.Deserialize(@"d:\blog_con_info.xml");


             for (int i = 0; i < posts.Length; i++)
             {
                 var original = posts[i];
                 var loaded = loaded_posts[i];
                 Assert.AreEqual(original.Description,loaded.Description);
                 Assert.AreEqual(original.DateCreated, loaded.DateCreated);
                 Assert.AreEqual(original.PostID, loaded.PostID);
                 Assert.AreEqual(original.Title, loaded.Title);
                 
             }
        }
    }
}
