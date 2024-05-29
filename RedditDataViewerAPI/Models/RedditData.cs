using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RedditDataViewerAPI.Models
{
    public class RedditData
    {
        public string SubReddit { get; set; }
        public string AuthorFullName { get; set; }

        public string Title { get; set; }

        public string UpvoteRatio { get; set; }
        public string UPS { get; set; }

        public int Score { get; set; }

        public DateTime Created { get; set; }

        public int NumComments { get; set; }

        public string After { get; set; }

    }
}
