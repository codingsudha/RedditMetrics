using Newtonsoft.Json;
using RedditDataViewerAPI.Interfaces;
using RedditDataViewerAPI.Models;
using RedditDataViewerAPI.Providers;
using System.Net.Http.Headers;

namespace RedditDataViewerAPI.Services
{
    public class RedditService : IRedditService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RedditService> _logger;
        private readonly HttpClient _httpClient;
        private string _after = "";

        public RedditService(IConfiguration configuration, ILogger<RedditService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
        }
        public RedditService(IConfiguration configuration, ILogger<RedditService> logger, HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        public void RedditServiceForAllPages()
        {
            while (GlobalData.CreatedDate >= GlobalData.ServiceStartTime)
            {
                _logger.LogInformation(GlobalData.CurrentCallCount.ToString());
                if (GlobalData.CurrentCallCount < 5)
                {
                    _logger.LogInformation("Waiting for 6 sec" + GlobalData.CurrentCallCount);
                    
                    Task.Delay(600).Wait(); // wait 6 sec
                }
                else
                {
                    try
                    {
                        CallRedditService();
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, $"Error calling Reddit Service: {ex.Message}");
                    }

                }
            }

        }

        public void CallRedditService()
        {
                string token = _configuration["RedditToken"];
                string applicationName = _configuration["ApplicationName"];
                string redditUsername = _configuration["RedditUsername"];

                _httpClient.DefaultRequestHeaders.Add("User-Agent", $"{applicationName} (by /u/{redditUsername}) v1.0.0"); // Replace placeholders
                                                                                                                           //var url = $"https://oauth.reddit.com/r/{subreddit}/new?limit=100&after={GlobalData.After}";

                var request = new HttpRequestMessage(HttpMethod.Get, GetFullUrl());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage httpResponseMessage = _httpClient.Send(request);
                httpResponseMessage.EnsureSuccessStatusCode();
                var responseStream = httpResponseMessage.Content.ReadAsStream();
                using var responseString = new StreamReader(responseStream);
                dynamic response = responseString.ReadToEnd();
                GlobalData.CurrentCallCount = Double.Parse(httpResponseMessage.Headers.GetValues("x-ratelimit-remaining").FirstOrDefault());

                MapRedditMatrixToModel(response);
        }

        private string GetFullUrl()
        {
            var url = $"{_configuration["BaseRedditApiUrl"]}/r/{_configuration["Subreddit"]}/new?limit=100&after={_after}";
            _logger.LogInformation(url);
            return url;
        }
        public Dictionary<string, object> GetPosts()
        {
            var userList = GlobalData.RedditData.GroupBy(post => post.AuthorFullName)
                                    .Select(group => new { AuthorFullName = group.Key, Count = group.Count() });

            var results = new Dictionary<string, object>()
                            {
                                { "PopularUser", userList.OrderByDescending(x => x.Count).FirstOrDefault()},
                                { "PopularPost", GlobalData.RedditData?.OrderByDescending(x => x.UPS).FirstOrDefault()},
                                { "DateTime" , DateTime.Now }
                            };
            return results;
        }

        private void MapRedditMatrixToModel(string response)
        {
            var redditListLocal = new List<RedditData>();

            // Deserialize the JSON string into a dynamic object
            dynamic jsonData = JsonConvert.DeserializeObject(response);

            // Access data based on the JSON structure
            foreach (var item in jsonData?.data.children)
            {
                GlobalData.CreatedDate = DateTime.UnixEpoch.AddSeconds(double.Parse(item.data.created_utc.ToString()));
                if (GlobalData.CreatedDate < GlobalData.ServiceStartTime)
                    break;

                redditListLocal.Add(new RedditData()
                {
                    SubReddit = item.data.subreddit,
                    UPS = item.data.ups,
                    UpvoteRatio = item.data.upvote_ratio,
                    AuthorFullName = item.data.author_fullname,
                    Title = item.data.title,
                    Score = item.data.score,
                    Created = GlobalData.CreatedDate,
                    NumComments = item.data.num_comments,
                    After = jsonData.data.after
                });

                _after = jsonData.data.after;
            }
            GlobalData.RedditData.AddRange(redditListLocal);

        }

    }
}
