using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogPS
{
    internal class BodyAnalysis
    {
        public List<HtmlAgilityPack.HtmlNode> ImgElements;
        public List<HtmlAgilityPack.HtmlNode> AHrefElements;


        public BodyAnalysis(HtmlAgilityPack.HtmlDocument doc)
        {
            // first handle the img links
            this.ImgElements = doc.DocumentNode.Descendants("img").ToList();
            foreach (var img_node in this.ImgElements)
            {
                var src = img_node.Attributes["src"];
                var url = src.Value.Trim();
            }

            // now get the a href links
            this.AHrefElements = doc.DocumentNode.Descendants("a").Where(el=>el.Attributes.Contains("href")).ToList();
           
        }
    }
}