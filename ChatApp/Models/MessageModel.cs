using System;
using MongoDB.Bson;

namespace ChatApp.Models
{
    public class MessageModel : IIdentified, IComparable
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }

        // Assuming Text and Author is not null
        public MessageModel(string text, string author)
        {
            Text = text;
            Author = author;
            Time = DateTime.Now;
        }

        public int CompareTo(object obj)
        {
            if (obj is MessageModel)
            {
                MessageModel other = (MessageModel)obj;
                return Time.CompareTo(other.Time);
            }
            return 0;
        }
    }
}