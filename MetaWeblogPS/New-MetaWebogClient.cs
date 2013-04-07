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
        [SMA.Parameter(Mandatory = true, Position = 0)]
        public string BlogURL;
        [SMA.Parameter(Mandatory = true, Position = 1)]
        public string MetaWeblogURL;
        [SMA.Parameter(Mandatory = true, Position = 2)] public string BlogID;
        [SMA.Parameter(Mandatory = true, Position = 3)] public string Username;
        [SMA.Parameter(Mandatory = true, Position = 4)] public string Password;

        protected override void ProcessRecord()
        {
            var con = new MetaWeblogSharp.BlogConnectionInfo(this.BlogURL,this.MetaWeblogURL, this.BlogID, this.Username,
                                                             this.Password);
            var client = new MetaWeblogSharp.Client(con);
            this.WriteObject(client);
        }
    }
}
