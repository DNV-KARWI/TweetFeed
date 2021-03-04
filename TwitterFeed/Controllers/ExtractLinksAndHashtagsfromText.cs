using System.Text.RegularExpressions;
using TwitterFeed.ViewModels;

namespace TwitterFeed.Controllers
{
    public class ExtractLinksAndHashtagsfromText
    {
        public TimelineViewModel TweetList { get; private set; }

        public ExtractLinksAndHashtagsfromText()
        {

        }

        public ExtractLinksAndHashtagsfromText(TimelineViewModel tweetList)
        {
            foreach (var tweet in tweetList.TweetViewModels)
            {
                GetLinksFromText(tweet);
                ReplaceLineBreak(tweet);
                GetHashtagsFromText(tweet);
                GetMentionsFromText(tweet);
            }
            TweetList = tweetList;
        }

        private void ReplaceLineBreak(ViewModelRoot tweet)
        {
            tweet.full_text = Regex.Replace(tweet.full_text, "\n", "<br>");
            if (tweet.retweeted_status != null)
            {
                tweet.retweeted_status.full_text = Regex.Replace(tweet.retweeted_status.full_text, "\n", "<br>");
            }
        }

        private void GetLinksFromText(ViewModelRoot tweet)
        {
            if (tweet.retweeted_status == null)
            {
                if (tweet.entities != null && tweet.entities.urls != null)
                {
                    foreach (var urls in tweet.entities.urls)
                    {
                        tweet.full_text = ReplaceLinkWithTag(tweet.full_text, urls);
                    }
                }
            }
            else
            {
                if (tweet.retweeted_status.entities != null && tweet.retweeted_status.entities.urls != null)
                {
                    foreach (var urls in tweet.retweeted_status.entities.urls)
                    {
                        tweet.retweeted_status.full_text = ReplaceLinkWithTag(tweet.retweeted_status.full_text, urls);
                    }
                }
            }
        }

        private string ReplaceLinkWithTag(string inputText, Models.Tweet.Url urls)
        {
            var text = Regex.Replace(inputText,
                @"((http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                delegate (Match m)
                {
                    if (urls.url == m.Value)
                    {
                        return string.Format("<a target='_blank' href='{0}'>{1}</a>", urls.url, urls.expanded_url);
                    }
                    return string.Empty;
                });
            return text;
        }

        private void GetHashtagsFromText(ViewModelRoot tweet)
        {
            if (tweet.retweeted_status == null)
            {
                if (tweet.entities != null && tweet.entities.hashtags != null)
                {
                    foreach (var hashtags in tweet.entities.hashtags)
                    {
                        tweet.full_text = ReplaceHashstagWithTag(tweet.full_text, "#", hashtags.text);
                    }
                }
            }
            else
            {
                if (tweet.retweeted_status.entities != null && tweet.retweeted_status.entities.hashtags != null)
                {
                    foreach (var hashtags in tweet.retweeted_status.entities.hashtags)
                    {
                        if (tweet.retweeted_status != null)
                        {
                            tweet.retweeted_status.full_text = ReplaceHashstagWithTag(tweet.retweeted_status.full_text, "#", hashtags.text);
                        }
                    }
                }
            }
        }

        private void GetMentionsFromText(ViewModelRoot tweet)
        {
            if (tweet.retweeted_status == null)
            {
                if (tweet.entities != null && tweet.entities.user_mentions != null)
                {
                    foreach (var mention in tweet.entities.user_mentions)
                    {
                        tweet.full_text = ReplaceHashstagWithTag(tweet.full_text, "@", mention.screen_name);
                    }
                }
            }
            else
            {
                if (tweet.retweeted_status.entities != null && tweet.retweeted_status.entities.user_mentions != null)
                {
                    foreach (var mention in tweet.retweeted_status.entities.user_mentions)
                    {
                        tweet.retweeted_status.full_text = ReplaceHashstagWithTag(tweet.retweeted_status.full_text, "@", mention.screen_name);
                    }
                }
            }
        }

        private string ReplaceHashstagWithTag(string inputText, string trailingMark, string replace)
        {
            var tag = string.Format("https://twitter.com/hashtag/{0}?src=hash", replace);
            return Regex.Replace(inputText, @trailingMark + replace, delegate (Match m)
            {
                return string.Format("<a target='_blank' href='{0}'><span>{1}</span><span >{2}</span></a>", tag, trailingMark, replace);
            });
        }
    }
}
