using System.Collections.Generic;
using TwitterFeed.Models;

namespace TwitterFeed.ViewModels
{
    public class TimelineViewModel
    {
        public List<TweetViewModel> TweetViewModels { get; set; }
    }

    public class TweetViewModel
    {
        public Datum UserTweet { get; set; }

        public Models.User.Datum User { get; set; }
    }
}
