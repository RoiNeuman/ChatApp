using System;
using System.Collections.Generic;
using ChatApp.Controllers;

namespace ChatApp.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public IList<MessageModel> Messages = new List<MessageModel>();
        public double AverageLattersPerMessage { get; set; }
        private double _lettersSum;

        public UserModel(string name)
        {
            Name = name;
            _lettersSum = 0;
            AverageLattersPerMessage = 0;
        }

        public MessageModel AddMessage(string text)
        {
            if (text == null)
                return null;
            MessageModel newMessage = new MessageModel(text, Name);
            Messages.Add(newMessage);
            GetAverageLattersPerMessage(newMessage);
            ChatController.MessagesList.Add(newMessage);
            Database.MessagesCollection.SaveAsync(newMessage);
            return newMessage;
        }

        public void GetAverageLattersPerMessage()
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

        // Overload with better time-complexity - O(1).
        private void GetAverageLattersPerMessage(MessageModel message)
        {
            if (Messages.Count == 0)
                AverageLattersPerMessage = 0;
            else
            {
                _lettersSum += message.Text.Length;
                AverageLattersPerMessage = Math.Round(_lettersSum / Messages.Count, 2);
            }
        }
    }
}