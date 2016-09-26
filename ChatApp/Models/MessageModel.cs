using System;

namespace ChatApp.Models
{
    public class MessageModel
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }

        // Assuming Text and Author is not null
        public MessageModel(string Text, string Author)
        {
            this.Text = Text;
            this.Author = Author;
            this.Time = DateTime.Now;
        }
    }
}