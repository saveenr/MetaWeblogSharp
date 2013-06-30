using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using HtmlAgilityPack;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Import", "MetaWeblogPostFromXml")]
    public class Import_MetaWeblogPostToXml : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)]
        public string Filename;

        protected override void ProcessRecord()
        {
            var posts = MetaWeblogSharp.PostInfo.Deserialize(this.Filename);
        }
    }
}