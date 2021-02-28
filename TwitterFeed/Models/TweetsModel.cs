using System;
using System.Collections.Generic;
namespace TwitterFeed.Models
{
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
        public List<Annotation> annotations { get; set; }
        public List<Url> urls { get; set; }
    }

    public class Data
    {
        public string source { get; set; }
        public DateTime created_at { get; set; }
        public Entities entities { get; set; }
        public bool possibly_sensitive { get; set; }
        public string author_id { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string lang { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }


}
