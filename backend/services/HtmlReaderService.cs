using AngleSharp;
using AngleSharp.Html.Parser;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace backend.services
{
    public class HtmlReader
    {
        private IHtmlParser _parser;

        public HtmlReader()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            _parser = context.GetService<IHtmlParser>();
        }

        public string ParseHtml (string url)
        {
            return BuildHtmlString(url);
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
