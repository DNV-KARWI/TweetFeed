using System;
using System.Collections.Generic;
using TwitterFeed.Models.Tweet;

namespace TwitterFeed.ViewModels
{
    public class TimelineViewModel
    {
        public List<ViewModelRoot> TweetViewModels { get; set; }
    }
    public class ViewModelRoot
    {
        public ViewModelRoot() { }

        public ViewModelRoot(Root root)
        {
            if (root != null)
            {
                this.Created_at = root.Created_at;
                this.id = root.id;
                this.id_str = root.id_str;
                this.full_text = root.full_text;
                this.entities = root.entities;
                this.extended_entities = root.extended_entities;
                this.source = root.source;
                this.user = root.user;
                this.contributors = root.contributors;
                this.retweet_count = root.retweet_count;
                if (root.retweeted_status != null)
                {
                    this.retweeted_status = new ViewModelRoot(root.retweeted_status);
                    this.retweeted_status.isRetweet = true;
                    this.retweeted_status.RetweetBy = root.user.name;
                }
            }
        }

        public DateTime Created_at { get; set; }
        public long id { get; set; }
        public string id_str { get; set; }
        public string full_text { get; set; }
        public Entities entities { get; set; }
        public ExtendedEntities extended_entities { get; set; }
        public string source { get; set; }
        public User user { get; set; }
        public object contributors { get; set; }
        public ViewModelRoot retweeted_status { get; set; }
        public int retweet_count { get; set; }
        public bool isRetweet { get; set; }
        public string RetweetBy { get; set; }
    }
}
