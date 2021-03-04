using System.Text.RegularExpressions;
using TwitterFeed.Models;
using TwitterFeed.ViewModels;

namespace TwitterFeed.Controllers
{
    public class ExtractLinksAndHashtagsfromText
    {
        public ExtractLinksAndHashtagsfromText()
        {

        }

        public ExtractLinksAndHashtagsfromText(TimelineViewModel tweetList)
        {
            GetLinksFromText(tweetList);
            ReplaceLineBreak(tweetList);
            GetHashtagsFromText(tweetList);
            GetMentionsFromText(tweetList);
        }

        public TimelineViewModel TweetList { get; private set; }

        private void ReplaceLineBreak(TimelineViewModel tweetList)
        {
            foreach (var tweet in tweetList.TweetViewModels)
            {
                tweet.full_text = Regex.Replace(tweet.full_text, "\n", "<br>");

                if(tweet.retweeted_status != null)
                {
                    tweet.retweeted_status.full_text = Regex.Replace(tweet.full_text, "\n", "<br>");
                }
            }
            TweetList = tweetList;
        }

        private void GetLinksFromText(TimelineViewModel tweetList)
        {
            foreach (var tweet in tweetList.TweetViewModels)
            {
                if (tweet.entities != null && tweet.entities.urls!= null)
                {
                    foreach (var urls in tweet.entities.urls)
                    {
                        tweet.full_text = ReplaceLinkWithTag(tweet.full_text, urls);

                        if (tweet.retweeted_status != null)
                        {
                            tweet.retweeted_status.full_text = ReplaceLinkWithTag(tweet.full_text, urls);
                        }
                    }
                }
            }
            TweetList = tweetList;
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

        private void GetHashtagsFromText(TimelineViewModel tweetList)
        {
            foreach (var tweet in tweetList.TweetViewModels)
            {
                if (tweet.entities != null && tweet.entities.hashtags != null)
                {
                    foreach (var hashtags in tweet.entities.hashtags)
                    {
                        tweet.full_text = ReplaceHashstagWithTag(tweet.full_text, "#", hashtags.text);
                        if (tweet.retweeted_status != null)
                        {
                            tweet.retweeted_status.full_text = ReplaceHashstagWithTag(tweet.full_text, "#", hashtags.text);
                        }
                    }
                }
            }
            TweetList = tweetList;
        }

        private void GetMentionsFromText(TimelineViewModel tweetList)
        {
            foreach (var tweet in tweetList.TweetViewModels)
            {
                if (tweet.entities != null && tweet.entities.user_mentions != null)
                {
                    foreach (var mention in tweet.entities.user_mentions)
                    {
                        tweet.full_text = ReplaceHashstagWithTag(tweet.full_text, "@", mention.screen_name);
                        if (tweet.retweeted_status != null)
                        {
                            tweet.retweeted_status.full_text = ReplaceHashstagWithTag(tweet.full_text, "@", mention.screen_name);
                        }
                    }
                }
            }
            TweetList = tweetList;
        }

        private string ReplaceHashstagWithTag(string inputText, string trailingMark, string replace)
        {
            var tag = string.Format("https://twitter.com/hashtag/{0}?src=hash", replace);
            return Regex.Replace(inputText, @trailingMark + replace, delegate (Match m) {
                return string.Format("<a target='_blank' href='{0}'><span>{1}</span><span >{2}</span></a>", tag, trailingMark, replace);
            });
        }
    }
}
