using backend.Dtos;
using System.Collections.Generic;

namespace backend.Requests
{
    public class ConfigurationPayload
    {
        public Dictionary<string, string> PosterSizes { get; set; }
        public Dictionary<string, string> BackDropSizes { get; set; }
        public Dictionary<string, string> LogoSizes { get; set; }
        public BaseUrlsDto BaseUrls { get; set; }
    }
}
