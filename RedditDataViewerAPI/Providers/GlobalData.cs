using RedditDataViewerAPI.Models;

namespace RedditDataViewerAPI.Providers
{
    public static class GlobalData
    {
        private static DateTime _createdDate = DateTime.UtcNow;
        private static double _currentCallCount = 600;
        public readonly static DateTime _serviceStartTime = DateTime.UtcNow.Subtract(TimeSpan.FromHours(3));
        private static List<RedditData> _redditData = new List<RedditData>();
        private static DateTime _lastCallTime = DateTime.UtcNow;
        private static int _callCount = 0;
        private static bool _firstCall = true;
        public static DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public static double CurrentCallCount
        {
            get { return _currentCallCount; }
            set { _currentCallCount = value; }
        }

        public static DateTime ServiceStartTime
        {
            get { return _serviceStartTime; }
        }

        public static List<RedditData> RedditData
        {
            get { return _redditData; }
            set { _redditData = value; }
        }

        public static DateTime LastCallTime
        {
            get { return _lastCallTime; }
            set { _lastCallTime = value; }
        }

        public static int CallCount
        {
            get { return _callCount; }
            set { _callCount = value; }
        }

        public static bool FirstCall
        {
            get { return _firstCall; }
            set { _firstCall = value; }
        }

    }
}
