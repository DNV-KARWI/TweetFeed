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

        public Root GetHashtagsFromText(Root tweetList)
        {
            foreach (var tweet in tweetList.data)
            {
                if (tweet.entities != null && tweet.entities.hashtags != null)
                {
                    foreach (var hashtags in tweet.entities.hashtags)
                    {
                        tweet.text = ReplaceHashstagWithTag(tweet.text, hashtags);
                    }
                }
            }
            return tweetList;
        }

        public string ReplaceHashstagWithTag(string inputText, Hashtag hashtags)
        {
            var tag = string.Format("https://twitter.com/hashtag/{0}?src=hash", hashtags.tag);
            return Regex.Replace(inputText, @"#" + hashtags.tag, delegate (Match m) {
                return string.Format("<a href='{0}'><span>#</span><span >{1}</span></a>", tag, hashtags.tag);
            });
        }
    }
}
