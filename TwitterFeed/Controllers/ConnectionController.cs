using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterFeed.Models;

namespace TwitterFeed.Controllers
{
    public class ConnectionController : Controller
    {
        public class Connection
        {
            private string BaseUrl = "https://api.twitter.com/";
            private string Bearer = "AAAAAAAAAAAAAAAAAAAAAKC7NAEAAAAAwAOe89ElGLyldSJS%2BCfBmj8EVXI%3DkI88UTu9tZokP6ZfZyTYMKa0F7X6QYbxkGHQVIpTBidamJ9vv5";
            
            public async Task<Models.Timeline.Root> GetTimelineTweets()
            {
                await GetUserInfo();
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

                        return JsonSerializer.Deserialize <Models.Timeline.Root> (tweets);
                    }
                }
                return null;
            }

            public async Task<Models.User.Datum> GetUserInfo()
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
                    response = await client.GetAsync("https://api.twitter.com/2/users?ids=37676072&user.fields=profile_image_url").ConfigureAwait(false);

                    // Verification  
                    if (response.IsSuccessStatusCode)
                    {
                        var user = await response.Content.ReadAsStringAsync();
                        // Reading Response.  

                        return JsonSerializer.Deserialize<Models.User.Datum>(user);
                    }
                }
                return null;
            }

            public async Task<string> GetTtimelineTweetsIDs()
            {
                var root = await GetTimelineTweets();

                var strBuilder = new StringBuilder();
                foreach (var tweet in root.data)
                {
                    strBuilder.Append(tweet.id + ",");
                }
                var str = strBuilder.ToString();

                return str.TrimEnd(',');
            }

            public async Task<Root> GetAllTweetsContent()
            {
                var tweetsIDs = await GetTtimelineTweetsIDs();

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
}
