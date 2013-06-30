using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Publish", "MetaWeblogMediaObject")]
    public class Publish_MetaWeblogMediaObject : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)] public MetaWeblogSharp.Client Client;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)] public string Name;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 2)] public string Filename;
        [System.Management.Automation.Parameter(Mandatory = true, Position = 3)] public string Type;
        [System.Management.Automation.Parameter(Mandatory = false)] public System.Management.Automation.SwitchParameter Publish = true;

        protected override void ProcessRecord()
        {
            var bytes = System.IO.File.ReadAllBytes(this.Filename);
            var moi = this.Client.NewMediaObject(this.Name, this.Type, bytes);
            this.WriteObject(moi);
        }
    }
}