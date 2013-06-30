using System.Collections.Generic;
using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Publish", "MetaWeblogPost")]
    public class Publish_MetaWeblogPost : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)] 
        public MetaWeblogSharp.Client Client;

        [System.Management.Automation.Parameter(ParameterSetName="props",Mandatory = true, Position = 1)] 
        public string Title;
        
        [System.Management.Automation.Parameter(ParameterSetName = "props", Mandatory = true, Position = 2)]
        public string Description;
        
        [System.Management.Automation.Parameter(ParameterSetName="props", Mandatory = false)]
        public List<string> Categories;
        
        [System.Management.Automation.Parameter(ParameterSetName = "props", Mandatory = false)]
        public System.DateTime DateCreated = new System.DateTime(9999, 1, 1);

        [System.Management.Automation.Parameter(Mandatory = false)]
        public System.Management.Automation.SwitchParameter Draft = false;
        
        protected override void ProcessRecord()
        {
            System.DateTime? dt=null;

            if (DateCreated.Year != 9999)
            {
                this.WriteVerbose("using date created");
                dt = this.DateCreated;
            }
            var postid = this.Client.NewPost(this.Title, this.Description, this.Categories, !this.Draft, dt);
            this.WriteObject(postid);                
        }
    }
}