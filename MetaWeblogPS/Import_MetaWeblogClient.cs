using System.Management.Automation;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Import", "MetaWeblog")]
    public class Import_MetaWeblogClient : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)]
        public string Filename;

        protected override void ProcessRecord()
        {
            var con = MetaWeblogSharp.BlogConnectionInfo.Load(this.Filename);
            var client = new MetaWeblogSharp.Client(con);
            this.WriteObject(client);
        }
    }
}