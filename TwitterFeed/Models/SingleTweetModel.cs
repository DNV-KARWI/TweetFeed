using System;
using System.Collections.Generic;
namespace TwitterFeed.Models
{
    public class Mention
    {
        public int start { get; set; }
        public int end { get; set; }
        public string username { get; set; }
    }

    public class Hashtag
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class Annotation
    {
        public int start { get; set; }
        public int end { get; set; }
        public double probability { get; set; }
        public string type { get; set; }
        public string normalized_text { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Url
    {
        public int start { get; set; }
        public int end { get; set; }
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public List<Image> images { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string unwound_url { get; set; }
    }

    public class Entities
    {
        public List<Mention> mentions { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public List<Annotation> annotations { get; set; }
        public List<Url> urls { get; set; }
    }

    public class Attachments
    {
        public List<string> media_keys { get; set; }
    }

    public class Datum
    {
        public string author_id { get; set; }
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string conversation_id { get; set; }
        public Entities entities { get; set; }
        public string text { get; set; }
        public Attachments attachments { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
    }



}
