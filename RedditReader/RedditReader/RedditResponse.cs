using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RedditReader
{
    public class RedditResponse
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public List<RedditItem> Items { get; set; }

        public RedditResponse(XElement response)
        {
            var channel = response.Element("channel");
            if(channel == null) throw  new Exception("Unexpected xml");
            
            Title = (string) channel.Element("title");
            Link = (string)channel.Element("link");
            

            Items = new List<RedditItem>();
            foreach (var item in channel.Elements("item").Where(i => i.Element(Ns.Media + "thumbnail") != null))
            {
                Items.Add(new RedditItem(item));
            }
        }
    }
}