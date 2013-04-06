using System.Collections.Generic;
using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Edit", "MetaWeblogPost")]
    public class Edit_MetaWeblogPost : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)] public string PostID;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 2)] public string Title;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 3)] public string Description;
        [System.Management.Automation.Parameter(Mandatory = false)] public List<string> Categories;
        [System.Management.Automation.Parameter(Mandatory = false)]
        public System.Management.Automation.SwitchParameter Draft = false;
        protected override void ProcessRecord()
        {
            var success = this.Client.EditPost(this.PostID, this.Title, this.Description, this.Categories, !this.Draft);
            this.WriteObject(success);
        }
    }
}