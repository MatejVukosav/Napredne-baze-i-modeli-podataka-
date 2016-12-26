using System;
using System.Collections.Generic;

namespace NBMP_1.projekt.Mongo
{
    public class News
    {
        public News()
        {
            created = DateTime.Now;
        }

        public Guid id = Guid.NewGuid();

        //     public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string author { get; set; }

        public string image { get; set; }
        public DateTime created { get; set; }
        public List<Comment> comments { get; set; }

        public class Comment
        {
            public Comment()
            {
                created = DateTime.Now;
            }
            public Guid id = Guid.NewGuid();
            public DateTime created { get; set; }
            public string text { get; set; }
        }
    }
}