namespace RedditDataViewerAPI.Interfaces
{
    public interface IRedditService
    {
        public void RedditServiceForAllPages();

        public void CallRedditService();

        public Dictionary<string, object> GetPosts();

    }
}
