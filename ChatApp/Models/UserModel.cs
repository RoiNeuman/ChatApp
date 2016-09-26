using System;
using System.Collections.Generic;
using ChatApp.Controllers;
using ChatApp.Hubs.Charts;

namespace ChatApp.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public IList<MessageModel> Messages = new List<MessageModel>();
        public double AverageLattersPerMessage { get; set; }

        public UserModel(string name)
        {
            Name = name;
            AverageLattersPerMessage = 0;
        }

        public MessageModel AddMessage(string text)
        {
            if (text == null)
                return null;
            MessageModel newMessage = new MessageModel(text, Name);
            Messages.Add(newMessage);
            GetAverageLattersPerMessage();
            ChatController.MessagesList.Add(newMessage);
            return newMessage;
        }

        private void GetAverageLattersPerMessage()
        {
            if (Messages.Count == 0)
                AverageLattersPerMessage = 0;
            else
            {
                double total = 0;
                foreach (MessageModel message in Messages)
                {
                    total += message.Text.Length;
                }
                AverageLattersPerMessage = Math.Round(total/Messages.Count, 2);
            }
        }
    }
}