using System;
using System.Linq;
using System.Xml.Linq;

namespace RedditReader
{
    public class RedditItem
    {
        public string Title { get; set; }
        public string Link { get; set; }

        public string Guid { get; set; }
        public DateTime PubDate { get; set; }
        
        public string Thumbnail { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public RedditItem()
        {
            
        }

        public RedditItem(XElement item)
            : this()
        {
            Title = (string) item.Element("title");
            Link = (string) item.Element("link");
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
            
            Description = (string) item.Element("description");

            var thumbnailNode = item.Element(Ns.Media + "thumbnail");
            if (thumbnailNode != null)
            {
                Thumbnail = (string)thumbnailNode.Attribute("url");
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                var desc = XDocument.Parse(Description).Root;
                var links = desc.Descendants("a");
                var imageLink = links.FirstOrDefault(e => (string) e == "[link]");
                if (imageLink != null)
                {
                    Image = (string)imageLink.Attribute("href");
                }
            }
        }
    }
}