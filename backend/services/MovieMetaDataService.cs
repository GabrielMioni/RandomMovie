using backend.Models.Deserializers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Web;

namespace backend.Services
{
    public class MovieMetaDataService
    {
        private readonly IConfiguration _config;
        private readonly string ApiKey;
        private readonly string RootUrl;

        public MovieMetaDataService(IConfiguration config)
        {
            _config = config;
            ApiKey = _config["MovieDbApi:ApiKey"];
            RootUrl = _config["MovieDbApi:Url"];
        }

        public Credits GetMovieCreditsByApiId(int movieId)
        {
            var response = GetResponseFromMovieApi($"movie/{movieId}/credits", null);
            var cast = JsonSerializer.Deserialize<Credits>(response);
            return cast;
        }

        public string SearchMovieByDirector (string directorName)
        {
            var response = GetResponseFromMovieApi("search/person", directorName);

            return response;           
        }

        public MovieSearch SearchMovieByTitle (string title)
        {
            var response = GetResponseFromMovieApi("search/movie", title);
            var movieSearchResponse = JsonSerializer.Deserialize<MovieSearch>(response);

            return movieSearchResponse;
        }

        private string GetResponseFromMovieApi (string endpoint, string queryValue)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "query", queryValue }
            };

            return SendRequest(endpoint, queryParams);
        }

        private string SendRequest (string endpoint, Dictionary<string, string> queryParams = null)
        {
            var uri = $"{RootUrl}/{endpoint}";

            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (queryParams != null)
            {
                foreach (var keyValue in queryParams)
                {
                    query.Add(keyValue.Key, keyValue.Value);
                }
            }

            query.Add("api_key", ApiKey);
            uriBuilder.Query = query.ToString();
            uri = uriBuilder.ToString();

            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpRequest.Accept = "application/json";

            var response = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
