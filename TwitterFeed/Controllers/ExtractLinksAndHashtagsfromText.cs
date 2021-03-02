using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterFeed.Models;

namespace TwitterFeed.Controllers
{
    public class ExtractLinksAndHashtagsfromText
    {
        public Root GetLinksFromText(Root tweetList)
        {
            foreach (var tweet in tweetList.data)
            {
                if (tweet.entities != null && tweet.entities.urls!= null)
                {
                    foreach (var urls in tweet.entities.urls)
                    {
                        tweet.text = ReplaceLinkWithTag(tweet.text, urls);
                    }
                }
            }
            return tweetList;
        }

        public string ReplaceLinkWithTag(string inputText, Url urls)
        {
            return Regex.Replace(inputText, urls.url, delegate (Match m) {
                return string.Format("<a target='_blank' href='{0}'>{1}</a>", m.Value, urls.display_url);
            });
        }
    }
}
