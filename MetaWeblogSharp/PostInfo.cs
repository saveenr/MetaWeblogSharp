namespace MetaWeblogSharp
{
    public class PostInfo
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string PostID { get; set; }
        public string UserID { get; set; }
        public int CommentCount { get; set; }
        public string PostStatus { get; set; }
        public string PermaLink { get; set; }
        public string Description { get; set; }

        public object RawData { get; set; }
    }
}