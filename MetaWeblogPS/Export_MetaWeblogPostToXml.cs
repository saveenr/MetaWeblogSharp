using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using HtmlAgilityPack;

namespace MetaWeblogPS
{
    [System.Management.Automation.Cmdlet("Export", "MetaWeblogPostToXml")]
    public class Export_MetaWeblogPostToXml : Cmdlet
    {
        [System.Management.Automation.Parameter(Mandatory = true, Position = 0)]
        public MetaWeblogSharp.PostInfo[] Posts;

        [System.Management.Automation.Parameter(Mandatory = true, Position = 1)]
        public string Filename;

        protected override void ProcessRecord()
        {
            MetaWeblogSharp.PostInfo.Serialize(this.Posts,this.Filename);
        }
    }
}