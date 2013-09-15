// ----------------------
//  Tests - Test1.cs 
//  mmachado - 9/15/2013 
// ----------------------

using System;
using System.Linq;
using System.Net;
using xSolon.Instructions;
using xSolon.Instructions.DTO;
using xSolon.Instructions.GUI;

public class MetaWeblogSharpSample1 : AbstractInstruction
{
    public override void Run()
    {
        var con1 = new MetaWeblogSharp.BlogConnectionInfo(
            "https://localhost/blog",
            "https://localhost/blog/_layouts/metaweblog.aspx",
            "blogId", // You might not know the id, we'll get it on next line.
            "",
            "");

        var browser = new MyBrowser(con1.BlogURL);

        var res = browser.ShowDialog();

        if (res == System.Windows.Forms.DialogResult.OK)
        {
            var container = new CookieContainer();

            container.Add(browser.fldCookies);

            con1.Cookies = container;
        }
        var client = new MetaWeblogSharp.Client(con1);

        // Get a list of all available blogs
        var blogs = client.GetUsersBlogs();

        // Send it to the 'Data Table' tab, if you need to see it
        // ResultTable = blogs.GetSerializedList().ListToTable();

        // Use the first blog
        var blog = blogs.First();

        NotifyInformation("Id:{0} Name:{1}", blog.BlogID, blog.BlogName);

        // API needs to the id of the blog
        client.BlogConnectionInfo.BlogID = blog.BlogID;

        // create a new post
        //string postid = client.NewPost("P1", "P1Content", 
        //	"Category 1;Category2".Split(';'), false, DateTime.Now);

        //NotifyInformation("New Post ID: {0}", postid);

        // update post
        //client.EditPost(postid,"new title","new description",null,false);

        var posts = client.GetRecentPosts(10);

        ResultTable = posts.GetSerializedList().ListToTable();

    }
}