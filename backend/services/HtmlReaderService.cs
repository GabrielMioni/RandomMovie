using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services
{
    public class HtmlReader
    {
        private IHtmlParser _parser;
        private IBrowsingContext _context;

        public HtmlReader()
        {
            var config = Configuration.Default;
            _context = BrowsingContext.New(config);
            // _parser = context.GetService<IHtmlParser>();
        }

        public async Task<IHtmlCollection<IElement>> ParseHtmlAsync (string url, string querySelector)
        {
            var source = BuildHtmlString(url);
            var document = await _context.OpenAsync(request => request.Content(source));

            return document.QuerySelectorAll(querySelector);
        }

        private string BuildHtmlString(string urlAddress)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "";
            }

            var receiveStream = response.GetResponseStream();
            var streamReader = string.IsNullOrWhiteSpace(response.CharacterSet)
                ? new StreamReader(receiveStream)
                : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

            var htmlString = streamReader.ReadToEnd();
            response.Close();
            streamReader.Close();

            return htmlString;
        }
    }
}
