using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterFeed.Models.Tweet;
using TwitterFeed.ViewModels;

namespace TwitterFeed.Controllers
{
    public class ConnectionController : Controller
    {
        private string BaseUrl = "https://api.twitter.com/";
        private string Bearer = "AAAAAAAAAAAAAAAAAAAAAKC7NAEAAAAAwAOe89ElGLyldSJS%2BCfBmj8EVXI%3DkI88UTu9tZokP6ZfZyTYMKa0F7X6QYbxkGHQVIpTBidamJ9vv5";

        public async Task<TimelineViewModel> Connection()
        {
            var timelineTweets = await GetTimelineTweets();
            var fetchTimelineTweets = GetTtimelineTweetsIDs(timelineTweets);
            var tweetList = await GetAllTweetsContent(fetchTimelineTweets);

            var feed = new TweetsController();
            feed.Convert(tweetList);

            return TweetsController.TweetList;
        }

        public async Task<Models.Timeline.Root> GetTimelineTweets()
        {
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Bearer);

                // Initialization.  
                var response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("2/users/37676072/tweets").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    var tweets = await response.Content.ReadAsStringAsync();
                    // Reading Response.  

                    return JsonSerializer.Deserialize<Models.Timeline.Root>(tweets);
                }
            }
            return null;
        }

        public List<string> GetTtimelineTweetsIDs(Models.Timeline.Root root)
        {
            var tweetIds = new List<string>();
            if (root != null && root.data != null)
            {
                foreach (var tweetId in root.data)
                {
                    tweetIds.Add(tweetId.id);
                }
            }
            return tweetIds;
        }

        public async Task<TimelineViewModel> GetAllTweetsContent(List<string> tweetsIDs)
        {
            var timelineViewModel = new TimelineViewModel();
            timelineViewModel.TweetViewModels = new List<ViewModelRoot>();
            foreach (var tweetId in tweetsIDs)
            {
                var tweet = await GetSingleTweetContent(tweetId);

                var viewModelRoot = new ViewModelRoot(tweet);
                timelineViewModel.TweetViewModels.Add(viewModelRoot);
            }
            return timelineViewModel;
        }

        public async Task<Root>GetSingleTweetContent(string tweetId)
        { 
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Bearer);

                // Initialization.  
                var response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("https://api.twitter.com/1.1/statuses/show.json?id=" + tweetId + "&tweet_mode=extended").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    var twitt = await response.Content.ReadAsStringAsync();
                    // Reading Response.  

                    return JsonSerializer.Deserialize<Root>(twitt);
                }
            }
            return null;
        }
    }
}
