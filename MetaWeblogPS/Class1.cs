using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SMA = System.Management.Automation;

namespace MetaWeblogPS
{
    [SMA.Cmdlet("New", "MetaWeblog")]
    public class New_MetaWeblogClient : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] public string MetaWeblogURL;
        [SMA.Parameter(Mandatory = true, Position = 1)] public string BlogID;
        [SMA.Parameter(Mandatory = true, Position = 2)] public string Username;
        [SMA.Parameter(Mandatory = true, Position = 3)] public string Password;

        protected override void ProcessRecord()
        {
            var con = new MetaWeblogSharp.BlogConnectionInfo(this.MetaWeblogURL, this.BlogID, this.Username,
                                                             this.Password);
            var client = new MetaWeblogSharp.Client(con);
            this.WriteObject(client);
        }
    }

    [SMA.Cmdlet("Import", "MetaWeblog")]
    public class Import_MetaWeblogClient : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)]
        public string Filename;

        protected override void ProcessRecord()
        {
            var con = MetaWeblogSharp.BlogConnectionInfo.Load(this.Filename);
            var client = new MetaWeblogSharp.Client(con);
            this.WriteObject(client);
        }
    }

    [SMA.Cmdlet("Get", "MetaWeblogPost")]
    public class Get_MetaWeblogPost : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [SMA.Parameter(Mandatory = true, Position = 1, ParameterSetName = "postid")] public string PostID;
        [SMA.Parameter(Mandatory = true, Position = 1, ParameterSetName = "recent")] public int NumPosts;

        protected override void ProcessRecord()
        {
            if (this.PostID != null)
            {
                var post = this.Client.GetPost(this.PostID);
                this.WriteObject(post);
            }
            else
            {
                var posts = this.Client.GetRecentPosts(this.NumPosts);
                this.WriteObject(posts);
            }
        }
    }

    [SMA.Cmdlet("Remove", "MetaWeblogPost")]
    public class Remove_MetaWeblogPost : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [SMA.Parameter(Mandatory = true, Position = 1)] public string PostID;

        protected override void ProcessRecord()
        {
            var success = this.Client.DeletePost(this.PostID);
        }
    }

    [SMA.Cmdlet("Publish", "MetaWeblogPost")]
    public class Publish_MetaWeblogPost : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] 
        public MetaWeblogSharp.Client Client;

        [SMA.Parameter(ParameterSetName = "post", Mandatory = true, Position = 1)]
        public MetaWeblogSharp.PostInfo Post;

        [SMA.Parameter(ParameterSetName="props",Mandatory = true, Position = 1)] 
        public string Title;
        
        [SMA.Parameter(ParameterSetName = "props", Mandatory = true, Position = 2)]
        public string Description;
        
        [SMA.Parameter(ParameterSetName="props", Mandatory = false)]
        public List<string> Categories;
        
        [SMA.Parameter(ParameterSetName = "props", Mandatory = false)]
        public System.DateTime DateCreated = new System.DateTime(9999, 1, 1);

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter Draft = false;
        
        protected override void ProcessRecord()
        {
            System.DateTime? dt=null;

            if (DateCreated.Year != 9999)
            {
                dt = this.DateCreated;
            }

            if (this.Post != null)
            {
                var postid = this.Client.NewPost(this.Post,this.Categories, !this.Draft);
                this.WriteObject(postid);                                
            }
            else
            {
                var postid = this.Client.NewPost(this.Title, this.Description, this.Categories, !this.Draft, dt);
                this.WriteObject(postid);                
            }
        }
    }

    [SMA.Cmdlet("Publish", "MetaWeblogMediaObject")]
    public class Publish_MetaWeblogMediaObject : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [SMA.Parameter(Mandatory = true, Position = 1)] public string Name;
        [SMA.Parameter(Mandatory = true, Position = 2)] public string Filename;
        [SMA.Parameter(Mandatory = true, Position = 3)] public string Type;
        [SMA.Parameter(Mandatory = false)] public SMA.SwitchParameter Publish = true;

        protected override void ProcessRecord()
        {
            var bytes = System.IO.File.ReadAllBytes(this.Filename);
            var moi = this.Client.NewMediaObject(this.Name, this.Type, bytes);
            this.WriteObject(moi);
        }
    }

    [SMA.Cmdlet("Edit", "MetaWeblogPost")]
    public class Edit_MetaWeblogPost : SMA.Cmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [SMA.Parameter(Mandatory = true, Position = 1)] public string PostID;
        [SMA.Parameter(Mandatory = true, Position = 2)] public string Title;
        [SMA.Parameter(Mandatory = true, Position = 3)] public string Description;
        [SMA.Parameter(Mandatory = false)] public List<string> Categories;
        [SMA.Parameter(Mandatory = false)] public SMA.SwitchParameter Publish = true;

        protected override void ProcessRecord()
        {
            var success = this.Client.EditPost(this.PostID, this.Title, this.Description, this.Categories, this.Publish);
            this.WriteObject(success);
        }
    }

    [SMA.Cmdlet("Export", "MetaWeblogPostToHTML")]
    public class Export_MetaWeblogPostToHTML : SMA.Cmdlet
    {
        public static string decode(string s)
        {
            return s; //.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        [SMA.Parameter(Mandatory = true, Position = 0)]
        public MetaWeblogSharp.PostInfo Post;

        [SMA.Parameter(Mandatory = true, Position = 1)]
        public string Filename;

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter WriteMediaObjects;

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter WriteMetaData = true;

        protected override void ProcessRecord()
        {
            var settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            var w = System.Xml.XmlWriter.Create(this.Filename, settings);

            w.WriteStartDocument();
            w.WriteDocType("html", null, null, null);
            w.WriteStartElement("html");
            w.WriteStartElement("head");
            w.WriteEndElement(); // </head>
            w.WriteStartElement("body");
            
            var post = this.Post;

            Console.WriteLine(post.Title);
            var title = decode(post.Title);
            w.WriteElementString("h1", title);
            if (this.WriteMetaData)
            {
                w.WriteElementString("p", string.Format("Title: {0}", post.Title ?? ""));
                w.WriteElementString("p", string.Format("Date Created: {0}", post.DateCreated.HasValue ? post.DateCreated.Value.ToString() : ""));
                w.WriteElementString("p", string.Format("ID: {0}", post.PostID));
                w.WriteElementString("p", string.Format("Status: {0}", post.PostStatus ?? ""));
                w.WriteElementString("p", string.Format("Link: {0}", post.Link ?? ""));
                w.WriteElementString("p", string.Format("PermaLink: {0}", post.PermaLink ?? ""));
            }
            w.WriteStartElement("a");
            w.WriteAttributeString("href", post.PermaLink ?? "");
            w.WriteString(post.PermaLink ?? "");
            w.WriteEndElement();

            var tbody = post.Description; 
            var new_doc = new HtmlAgilityPack.HtmlDocument();
            new_doc.LoadHtml(tbody);



            if (this.WriteMediaObjects)
            {
                // first handle the img links
                var ba = new BodyAnalysis(new_doc);
                foreach (var img_node in ba.ImgElements)
                {
                    var src = img_node.Attributes["src"];
                    var url = src.Value.Trim();
                    this.WriteVerbose("Downloading URL: " + url);

                    var bits = DownloadURLAsBytes(url);

                    var uri = new System.Uri(url);

                    var a = System.IO.Path.GetFileName(uri.LocalPath);
                    this.WriteVerbose("A: " + a);
                    var files_folder = this.Filename + "_files";
                    this.WriteVerbose("Files Folder: " + files_folder);
                    if (!System.IO.Directory.Exists(files_folder))
                    {
                        this.WriteVerbose("CREATED: " + files_folder);
                        System.IO.Directory.CreateDirectory(files_folder);
                    }

                   
                    var local_img_fname = System.IO.Path.Combine(files_folder, a);
                    this.WriteVerbose("local fname : " + local_img_fname);
                    System.IO.File.WriteAllBytes(local_img_fname, bits);

                    src.Value = local_img_fname;
                }

                // no handle a a href that are images
                foreach (var a_node in ba.AHrefElements)
                {
                    var src = a_node.Attributes["href"];
                    var url = src.Value.Trim();
                    this.WriteVerbose("Downloading URL: " + url);

                    var url_lc = url.ToLower();
                    var uri_lc = new System.Uri(url_lc);
                    if (uri_lc.LocalPath.EndsWith(".png") || uri_lc.LocalPath.EndsWith(".jpg"))
                    {
                        var bits = DownloadURLAsBytes(url);

                        var uri = new System.Uri(url);

                        var a = System.IO.Path.GetFileName(uri.LocalPath);
                        this.WriteVerbose("A: " + a);
                        var files_folder = this.Filename + "_files";
                        this.WriteVerbose("Files Folder: " + files_folder);
                        if (!System.IO.Directory.Exists(files_folder))
                        {
                            this.WriteVerbose("CREATED: " + files_folder);
                            System.IO.Directory.CreateDirectory(files_folder);
                        }


                        var local_img_fname = System.IO.Path.Combine(files_folder, a);
                        this.WriteVerbose("local fname : " + local_img_fname);
                        System.IO.File.WriteAllBytes(local_img_fname, bits);

                        src.Value = local_img_fname;

                    }

                }

            }
            w.WriteRaw(new_doc.DocumentNode.InnerHtml);
            w.WriteEndElement(); // </body>
            w.WriteEndElement(); // </html>
            w.WriteEndDocument();
            w.Flush();
            w.Close();
        }

        private static byte[] DownloadURLAsBytes(string url)
        {
            var wc = new System.Net.WebClient();
            var bits = wc.DownloadData(url);
            return bits;
        }


    }


    public class BodyAnalysis
    {
        public List<HtmlAgilityPack.HtmlNode> ImgElements;
        public List<HtmlAgilityPack.HtmlNode> AHrefElements;

        public BodyAnalysis(HtmlAgilityPack.HtmlDocument doc)
        {
                // first handle the img links
                this.ImgElements = doc.DocumentNode.Descendants("img").ToList();
                foreach (var img_node in this.ImgElements)
                {
                    var src = img_node.Attributes["src"];
                    var url = src.Value.Trim();
                }

                // now get the a href links
                this.AHrefElements = doc.DocumentNode.Descendants("a").Where(el=>el.Attributes.Contains("href")).ToList();
           
        }
    }
}
