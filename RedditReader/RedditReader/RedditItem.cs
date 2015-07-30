using System;
using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace RedditReader
{
    public class RedditItem
    {
        public string Title { get; set; }
        public string RedditLink { get; set; }

        public string Guid { get; set; }
        public DateTime PubDate { get; set; }
        
        public string Thumbnail { get; set; }

        public string Description { get; set; }

        public string ExternalLink { get; set; }

        public RedditItem()
        {
            
        }

        public RedditItem(XElement item)
            : this()
        {
            Title = (string) item.Element("title");
            RedditLink = (string) item.Element("link");
            Guid = (string) item.Element("guid");

            if (!string.IsNullOrEmpty(Guid))
            {
                var parts = Guid.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "comments")
                    {
                        Guid = parts[i + 1];
                        break;
                    }
                }
            }

            PubDate = Utility.XmlToDate((string) item.Element("pubDate"));

            var thumbnailNode = item.Element(Ns.Media + "thumbnail");
            if (thumbnailNode != null)
            {
                Thumbnail = (string)thumbnailNode.Attribute("url");
            }

            //Parse out the description node.
            var descriptionText = (string) item.Element("description");
            if (!string.IsNullOrWhiteSpace(descriptionText))
            {
                var desc = new HtmlDocument();
                desc.LoadHtml(descriptionText);

                var links = desc.DocumentNode.Descendants("a");

                var linkNode = links.FirstOrDefault(e => e.InnerText == "[link]");
                if (linkNode != null)
                {
                    ExternalLink = (string)linkNode.GetAttributeValue("href", "");
                }

                if (!string.IsNullOrWhiteSpace(Thumbnail))
                {
                    //This is a image link
                    Description = "";
                }
                else
                {
                    //This is a text link
                    Description =
                        desc.DocumentNode.Descendants("div")
                            .Single(x => x.GetAttributeValue("class", "") == "md")
                            .InnerText;
                }
            }
        }
    }
}