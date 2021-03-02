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
        public void GetLinksFromText(Root tweetList)
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
        }

        public string ReplaceLinkWithTag(string inputText, Url urls)
        {
            return Regex.Replace(inputText, urls.url, delegate (Match m) {
                return string.Format("<a target='_blank' href='{0}'>{1}</a>", m.Value, urls.display_url);
            });
        }

        public void GetHashtagsFromText(Root tweetList)
        {
            foreach (var tweet in tweetList.data)
            {
                if (tweet.entities != null && tweet.entities.hashtags != null)
                {
                    foreach (var hashtags in tweet.entities.hashtags)
                    {
                        tweet.text = ReplaceHashstagWithTag(tweet.text, "#", hashtags.tag);
                    }
                }
            }
        }

        public void GetMentionsFromText(Root tweetList)
        {
            foreach (var tweet in tweetList.data)
            {
                if (tweet.entities != null && tweet.entities.mentions != null)
                {
                    foreach (var mentions in tweet.entities.mentions)
                    {
                        tweet.text = ReplaceHashstagWithTag(tweet.text, "@", mentions.username);
                    }
                }
            }
        }

        public string ReplaceHashstagWithTag(string inputText, string trailingMark, string replace)
        {
            var tag = string.Format("https://twitter.com/hashtag/{0}?src=hash", replace);
            return Regex.Replace(inputText, @trailingMark + replace, delegate (Match m) {
                return string.Format("<a href='{0}'><span>{1}</span><span >{2}</span></a>", tag, trailingMark, replace);
            });
        }
    }
}
