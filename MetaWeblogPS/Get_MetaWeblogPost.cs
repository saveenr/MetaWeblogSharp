using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Get", "MetaWeblogPost")]
    public class Get_MetaWeblogPost : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1, ParameterSetName = "postid")] public string PostID;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1, ParameterSetName = "recent")] public int NumPosts;

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
}