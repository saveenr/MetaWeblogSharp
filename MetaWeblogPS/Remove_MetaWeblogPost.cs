using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Remove", "MetaWeblogPost")]
    public class Remove_MetaWeblogPost : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)] public string PostID;

        protected override void ProcessRecord()
        {
            var success = this.Client.DeletePost(this.PostID);
        }
    }
}