using System.Text.RegularExpressions;
using TwitterFeed.Models;

namespace TwitterFeed.Controllers
{
    public class ExtractLinksAndHashtagsfromText
    {
        public ExtractLinksAndHashtagsfromText()
        {

        }

        public ExtractLinksAndHashtagsfromText(Root tweetList)
        {
            GetLinksFromText(tweetList);
            GetHashtagsFromText(tweetList);
            GetMentionsFromText(tweetList);
        }

        public Root TweetList { get; private set; }

        private void GetLinksFromText(Root tweetList)
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
            TweetList = tweetList;
        }

        private string ReplaceLinkWithTag(string inputText, Url urls)
        {
            return Regex.Replace(inputText, urls.url, delegate (Match m) {
                return string.Format("<a target='_blank' href='{0}'>{1}</a>", m.Value, urls.display_url);
            });
        }

        private void GetHashtagsFromText(Root tweetList)
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
            TweetList = tweetList;
        }

        private void GetMentionsFromText(Root tweetList)
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
            TweetList = tweetList;
        }

        private string ReplaceHashstagWithTag(string inputText, string trailingMark, string replace)
        {
            var tag = string.Format("https://twitter.com/hashtag/{0}?src=hash", replace);
            return Regex.Replace(inputText, @trailingMark + replace, delegate (Match m) {
                return string.Format("<a href='{0}'><span>{1}</span><span >{2}</span></a>", tag, trailingMark, replace);
            });
        }
    }
}
