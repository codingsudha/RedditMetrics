using FakeItEasy;
using Xunit;
using FluentAssertions;
using RedditDataViewerAPI.Services;
using RedditDataViewerAPI.Interfaces;
using RedditDataViewerAPI.Providers;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Reddit.Models.Internal;
using Moq;
using Moq.Protected;
using System.Reflection.PortableExecutable;
using RedditDataViewerAPI.Models;

namespace RedditViewerAPITest.Service
{
    public class RedditServiceTest
    {
        private readonly HttpClient _mockHttpClient;
        private readonly IConfiguration _mockConfiguration;
        private readonly ILogger<RedditService> _mockLogger;
        private readonly HttpResponseMessage _mockHttpResponse;
        private readonly HttpRequestMessage _mockHttpRequest;
        public RedditServiceTest()
        {
            _mockHttpClient = A.Fake<HttpClient>();
            _mockConfiguration = A.Fake<IConfiguration>();
            _mockLogger = A.Fake<ILogger<RedditService>>();

            _mockHttpResponse = A.Fake<HttpResponseMessage>();
            _mockHttpRequest = A.Fake<HttpRequestMessage>();
        }

        [Fact]
        public void CallRedditService_Should_Populate_RedditDataList()
        {
            GlobalData.RedditData = new List<RedditData>();
            var result = "{\r\n  \"kind\": \"Listing\",\r\n  \"data\": {\r\n    \"after\": \"t3_1d2zzfh\",\r\n    \"dist\": 1,\r\n    \"modhash\": \"\",\r\n    \"geo_filter\": \"\",\r\n    \"children\": [\r\n      {\r\n        \"kind\": \"t3\",\r\n        \"data\": {\r\n          \"approved_at_utc\": null,\r\n          \"subreddit\": \"pics\",\r\n          \"author_fullname\": \"t2_ntgaxtlq8\",\r\n          \"title\": \"Early in the Morning \",\r\n          \"link_flair_richtext\": [],\r\n          \"subreddit_name_prefixed\": \"r/pics\",\r\n          \"hidden\": false,\r\n          \"downs\": 0,\r\n          \"hide_score\": true,\r\n          \"name\": \"t3_1d2zzfh\",\r\n          \"quarantine\": false,\r\n          \"link_flair_text_color\": \"dark\",\r\n          \"upvote_ratio\": 1,\r\n          \"subreddit_type\": \"public\",\r\n          \"ups\": 1,\r\n          \"total_awards_received\": 0,\r\n          \"score\": 1,\r\n          \"created\": 1716944843,\r\n          \"view_count\": null,\r\n          \"subreddit_id\": \"t5_2qh0u\",\r\n          \"author_is_blocked\": false,\r\n          \"id\": \"1d2zzfh\",\r\n          \"is_robot_indexable\": true,\r\n          \"report_reasons\": null,\r\n          \"author\": \"YeagerEren07\",\r\n          \"discussion_type\": null,\r\n          \"num_comments\": 1,\r\n          \"subreddit_subscribers\": 30833758,\r\n          \"created_utc\": 1716944843\r\n        }\r\n      },\r\n      {\r\n        \"kind\": \"t3\",\r\n        \"data\": {\r\n          \"approved_at_utc\": null,\r\n          \"subreddit\": \"pics\",\r\n          \"author_fullname\": \"t2_ntgaxtlq8\",\r\n          \"title\": \"Early in the Morning \",\r\n          \"link_flair_richtext\": [],\r\n          \"subreddit_name_prefixed\": \"r/pics\",\r\n          \"hidden\": false,\r\n          \"downs\": 0,\r\n          \"hide_score\": true,\r\n          \"name\": \"t3_1d2zzfh\",\r\n          \"quarantine\": false,\r\n          \"link_flair_text_color\": \"dark\",\r\n          \"upvote_ratio\": 9,\r\n          \"subreddit_type\": \"public\",\r\n          \"ups\": 5,\r\n          \"total_awards_received\": 0,\r\n          \"score\": 1,\r\n          \"created\": 1716944843,\r\n          \"view_count\": null,\r\n          \"subreddit_id\": \"t5_2qh0u\",\r\n          \"author_is_blocked\": false,\r\n          \"id\": \"1d2zzfh\",\r\n          \"is_robot_indexable\": true,\r\n          \"report_reasons\": null,\r\n          \"author\": \"YeagerEren07\",\r\n          \"discussion_type\": null,\r\n          \"num_comments\": 1,\r\n          \"subreddit_subscribers\": 30833758,\r\n          \"created_utc\": 1716944843\r\n        }\r\n      }\r\n    ],\r\n    \"before\": null\r\n  }\r\n}";
            using var response = new HttpResponseMessage()
            {
                Content = new StringContent(result,
                    Encoding.UTF8, "application/json")
            };
            var mockHandler = new Mock<HttpMessageHandler>();
            var mockedProtected = mockHandler.Protected();
            mockedProtected.Setup<HttpResponseMessage>("Send",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = response.Content,
                    Headers =
                        {
                            { "X-RateLimit-Remaining", "2" }
             }
                });



            var mockedHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://oauth.reddit.com/r/pics/new?limit=100&after=")
            };

           
            IRedditService _mockRedditService = new RedditService(_mockConfiguration, _mockLogger, mockedHttpClient);

            _mockRedditService.CallRedditService();

            GlobalData.RedditData.Count.Should().BeGreaterThan(1);
            
        }
        [Fact]
        public void GetPosts_Should_Return_PopularPost()
        {
            GlobalData.RedditData = new List<RedditData>();
            var result = "{\r\n  \"kind\": \"Listing\",\r\n  \"data\": {\r\n    \"after\": \"t3_1d2zzfh\",\r\n    \"dist\": 1,\r\n    \"modhash\": \"\",\r\n    \"geo_filter\": \"\",\r\n    \"children\": [\r\n      {\r\n        \"kind\": \"t3\",\r\n        \"data\": {\r\n          \"approved_at_utc\": null,\r\n          \"subreddit\": \"pics\",\r\n          \"author_fullname\": \"t2_ntgaxtlq8\",\r\n          \"title\": \"Early in the Morning \",\r\n          \"link_flair_richtext\": [],\r\n          \"subreddit_name_prefixed\": \"r/pics\",\r\n          \"hidden\": false,\r\n          \"downs\": 0,\r\n          \"hide_score\": true,\r\n          \"name\": \"t3_1d2zzfh\",\r\n          \"quarantine\": false,\r\n          \"link_flair_text_color\": \"dark\",\r\n          \"upvote_ratio\": 1,\r\n          \"subreddit_type\": \"public\",\r\n          \"ups\": 1,\r\n          \"total_awards_received\": 0,\r\n          \"score\": 1,\r\n          \"created\": 1716944843,\r\n          \"view_count\": null,\r\n          \"subreddit_id\": \"t5_2qh0u\",\r\n          \"author_is_blocked\": false,\r\n          \"id\": \"1d2zzfh\",\r\n          \"is_robot_indexable\": true,\r\n          \"report_reasons\": null,\r\n          \"author\": \"YeagerEren07\",\r\n          \"discussion_type\": null,\r\n          \"num_comments\": 1,\r\n          \"subreddit_subscribers\": 30833758,\r\n          \"created_utc\": 1716944843\r\n        }\r\n      },\r\n      {\r\n        \"kind\": \"t3\",\r\n        \"data\": {\r\n          \"approved_at_utc\": null,\r\n          \"subreddit\": \"pics\",\r\n          \"author_fullname\": \"t2_ntgaxtlq8\",\r\n          \"title\": \"Early in the Morning \",\r\n          \"link_flair_richtext\": [],\r\n          \"subreddit_name_prefixed\": \"r/pics\",\r\n          \"hidden\": false,\r\n          \"downs\": 0,\r\n          \"hide_score\": true,\r\n          \"name\": \"t3_1d2zzfh\",\r\n          \"quarantine\": false,\r\n          \"link_flair_text_color\": \"dark\",\r\n          \"upvote_ratio\": 9,\r\n          \"subreddit_type\": \"public\",\r\n          \"ups\": 5,\r\n          \"total_awards_received\": 0,\r\n          \"score\": 1,\r\n          \"created\": 1716944843,\r\n          \"view_count\": null,\r\n          \"subreddit_id\": \"t5_2qh0u\",\r\n          \"author_is_blocked\": false,\r\n          \"id\": \"1d2zzfh\",\r\n          \"is_robot_indexable\": true,\r\n          \"report_reasons\": null,\r\n          \"author\": \"YeagerEren07\",\r\n          \"discussion_type\": null,\r\n          \"num_comments\": 1,\r\n          \"subreddit_subscribers\": 30833758,\r\n          \"created_utc\": 1716944843\r\n        }\r\n      }\r\n    ],\r\n    \"before\": null\r\n  }\r\n}";
            using var response = new HttpResponseMessage()
            {
                Content = new StringContent(result,
                    Encoding.UTF8, "application/json")
            };
            var mockHandler = new Mock<HttpMessageHandler>();
            var mockedProtected = mockHandler.Protected();
            mockedProtected.Setup<HttpResponseMessage>("Send",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = response.Content,
                    Headers =
                        {
                            { "X-RateLimit-Remaining", "2" }
             }
                });



            var mockedHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://oauth.reddit.com/r/pics/new?limit=100&after=")
            };


            IRedditService _mockRedditService = new RedditService(_mockConfiguration, _mockLogger, mockedHttpClient);

            _mockRedditService.CallRedditService();
            var getPopular = _mockRedditService.GetPosts();

            getPopular.Should().NotBeNull();

            var popularPostAuthor = getPopular.GetValueOrDefault("PopularPost").As<RedditData>().AuthorFullName;
            Assert.Equal(popularPostAuthor, "t2_ntgaxtlq8");
        }

    }
}
