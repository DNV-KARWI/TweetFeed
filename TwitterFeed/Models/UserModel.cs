using System.Collections.Generic;

namespace TwitterFeed.Models.User
{
    public class Datum
    {
        public string profile_image_url { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
    }
}
