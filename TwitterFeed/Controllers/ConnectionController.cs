using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterFeed.Models;
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

            var extractLinksAndHashtagsfromText = new ExtractLinksAndHashtagsfromText();
            extractLinksAndHashtagsfromText.GetLinksFromText(tweetList);
            extractLinksAndHashtagsfromText.GetHashtagsFromText(tweetList);
            extractLinksAndHashtagsfromText.GetMentionsFromText(tweetList);

            var timelineViewModel = new TimelineViewModel();
            timelineViewModel.TweetViewModels = new List<TweetViewModel>();

            foreach (var tweet in tweetList.data)
            {
                var fetchedUser = await GetUserInfo(tweet.author_id);
                if (fetchedUser.data.Count > 0)
                {
                    TweetViewModel tweetViewModel = new TweetViewModel()
                    {
                        User = fetchedUser.data[0],
                        UserTweet = tweet
                    };
                    timelineViewModel.TweetViewModels.Add(tweetViewModel);
                }
            }
            return timelineViewModel;
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

        public async Task<Models.User.Root> GetUserInfo(string author_id)
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
                response = await client.GetAsync("https://api.twitter.com/2/users?ids=" + author_id + "&user.fields=profile_image_url").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsStringAsync();
                    // Reading Response.  
                    var ser = JsonSerializer.Deserialize<Models.User.Root>(user);
                    return JsonSerializer.Deserialize<Models.User.Root>(user);
                }
            }
            return null;
        }

        public string GetTtimelineTweetsIDs(Models.Timeline.Root root)
        {
            var strBuilder = new StringBuilder();
            foreach (var tweet in root.data)
            {
                strBuilder.Append(tweet.id + ",");
            }
            var str = strBuilder.ToString();

            return str.TrimEnd(',');
        }

        public async Task<Root> GetAllTweetsContent(string tweetsIDs)
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
                response = await client.GetAsync("https://api.twitter.com/2/tweets?ids="
                    + tweetsIDs
                    + "&tweet.fields=author_id,entities,attachments,conversation_id,created_at").ConfigureAwait(false);

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
