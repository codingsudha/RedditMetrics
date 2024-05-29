using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedditDataViewerAPI.Interfaces;

namespace RedditDataViewerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IRedditService _redditservice;
        public HomeController(IRedditService redditService)
        {
            _redditservice = redditService;
        }

        [HttpGet("RedditMetrics")]
        public ActionResult RedditMetricsForPopularPostAndPopularUser()
        {
                var result = _redditservice.GetPosts();
                return Ok(JsonConvert.SerializeObject(result));
            
        }
    }
}
