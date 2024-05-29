Populate these setting in appsettings.json file

   "RedditToken": "",
  "ApplicationName": "",
  "AuthorizationCode": "",
  "RedditUsername": "",
  "Subreddit": "pics",


  "LogFilePath": "C:\\Logs\\log-{Date}.txt"

  For Generating token, call post end point https://www.reddit.com/api/v1/access_token given username and password.

  For AuthorizationCode copy paste the following url and use the code generated from this
  https://www.reddit.com/api/v1/authorize?client_id=**APPLICATIONNAME**&response_type=code&state=somestring&redirect_uri=http://localhost:8080/redirect&duration=permanent&scope=identity edit flair history modconfig modflair mysubreddits read report save submit
