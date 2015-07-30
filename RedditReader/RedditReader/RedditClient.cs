using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedditReader
{
    public class RedditClient
    {
        private readonly bool _mustHaveThumbnails;
        private const string BASE_PATH = "http://reddit.com";
        private readonly List<string> _sources;
        
        private readonly HttpClient _client;
        private int _currentPosition = 0;
        private readonly int _pageSize;
        private string _after = null;

        public RedditClient(int pageSize, bool mustHaveThumbnails)
            : this(pageSize)
        {
            _mustHaveThumbnails = mustHaveThumbnails;
        }

        public RedditClient(int pageSize)
        {
            _pageSize = pageSize;
            _sources = new List<string>();
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0");
        }

        public void AddSource(string source)
        {
            if (!_sources.Contains(source))
            {
                _sources.Add(source);
            }
        }
        
        public async Task<IEnumerable<RedditItem>> Next()
        {
            _currentPosition += _pageSize;

            var sb = new StringBuilder();

            sb.AppendFormat("{0}/r/{1}.rss?limit={2}",
                BASE_PATH,
                string.Join("+", _sources),
                _currentPosition);

            if (_after != null)
            {
                sb.AppendFormat("&after=t3_{0}", _after);
            }
            
            var result = await _client.GetStringAsync(sb.ToString());
            
            var xDoc = XDocument.Parse(result);
            var response = new RedditResponse(xDoc.Root);

            _after = response.Items.Last().Guid;
            
            return response.Items;
        }
    }
}