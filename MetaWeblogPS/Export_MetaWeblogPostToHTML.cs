using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using HtmlAgilityPack;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Export", "MetaWeblogPostToHTML")]
    public class Export_MetaWeblogPostToHTML : Cmdlet
    {
        public static string decode(string s)
        {
            return s; //.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)]
        public MetaWeblogSharp.PostInfo Post;

        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)]
        public string Filename;

        [System.Management.Automation.Parameter(Mandatory = false)]
        public System.Management.Automation.SwitchParameter WriteMediaObjects;

        [System.Management.Automation.Parameter(Mandatory = false)]
        public System.Management.Automation.SwitchParameter WriteMetaData = true;

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

                var wc = new System.Net.WebClient();

                foreach (var img_node in ba.ImgElements)
                {
                    var src = img_node.Attributes["src"];
                    var url = src.Value.Trim();
                    var local_image_fname = DownLoadImage(url, wc);
                    src.Value = local_image_fname;

                }

                // no handle a a href that are images
                foreach (var a_node in ba.AHrefElements)
                {
                    var href = a_node.Attributes["href"];
                    var url = href.Value.Trim();

                    var ext = System.IO.Path.GetExtension(url).ToLower();
                    if (ext == ".jpg" || ext == ".png")
                    {
                        var local_image_fname = DownLoadImage(url, wc);
                        href.Value = local_image_fname;
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

        private string DownLoadImage(string url, WebClient wc)
        {
            var url_lc = url.ToLower();
            var uri_lc = new System.Uri(url_lc);
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
            wc.DownloadFile(url, local_img_fname);
            return local_img_fname;
        }
    }
}