using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterFeed.Models;

namespace TwitterFeed.Controllers
{
    public class ConnectionController : Controller
    {
        public class Connection
        {
            public async Task<Root> Connect()
            {
                using (var client = new HttpClient())
                {
                    // Setting Base address.  
                    client.BaseAddress = new Uri("https://api.twitter.com/");

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAAKC7NAEAAAAAwAOe89ElGLyldSJS%2BCfBmj8EVXI%3DkI88UTu9tZokP6ZfZyTYMKa0F7X6QYbxkGHQVIpTBidamJ9vv5");

                    // Initialization.  
                    HttpResponseMessage response = new HttpResponseMessage();

                    // HTTP GET  
                    response = await client.GetAsync("2/tweets/1275828087666679809?tweet.fields=attachments,author_id,created_at,entities,geo,id,in_reply_to_user_id,lang,possibly_sensitive,referenced_tweets,source,text,withheld").ConfigureAwait(false);

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
