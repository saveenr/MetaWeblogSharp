// ----------------------
//  MetaWeblogSharp - PostInfo.cs 
//  administrator - 6/29/2013 
// ----------------------

using System.Collections.Generic;

namespace MetaWeblogSharp
{
    public class PostInfo
    {
        public List<string> Categories = new List<string>();

        public string Title
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public System.DateTime? DateCreated
        {
            get;
            set;
        }

        public string PostID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public int CommentCount
        {
            get;
            set;
        }

        public string PostStatus
        {
            get;
            set;
        }

        public string PermaLink
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public static void Serialize(MetaWeblogSharp.PostInfo[] posts, string filename)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MetaWeblogSharp.PostInfo[]));
            var textWriter = new System.IO.StreamWriter(filename);
            serializer.Serialize(textWriter, posts);
            textWriter.Close();
        }

        public static MetaWeblogSharp.PostInfo[] Deserialize(string filename)
        {
            var fp = System.IO.File.OpenText(filename);
            var posts_serializer = new System.Xml.Serialization.XmlSerializer(typeof(MetaWeblogSharp.PostInfo[]));
            var loaded_posts = (MetaWeblogSharp.PostInfo[])posts_serializer.Deserialize(fp);
            fp.Close();
            return loaded_posts;
        }
    }
}