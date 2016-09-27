using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using ChatApp.Controllers;
using Newtonsoft.Json;
using static ChatApp.Controllers.ChatController;

namespace ChatApp.Models
{
    public class StatisticsModel
    {
        [JsonProperty("averageTimeForMessage")] private string _averageTimeForMessage;
        [JsonProperty("averageLettersPerMessage")] private double _averageLettersPerMessage;
        [JsonProperty("averageLettersPerUser")] private DynamicJsonObject _averageLettersPerUser;
        [JsonProperty("messagesPerHourChart")] private int[] _messagesPerHourChart;
        [JsonProperty("lettersPerHourChart")] private int[] _lettersPerHourChart;

        // Statics variables for better time-complexity
        private static TimeSpan _firstMessageTimeSpan;
        private static double _sumMessagesTimeSum;


        public StatisticsModel()
        {
            Init(null);
        }

        public void Init(MessageModel message)
        {
            // Each private method calculates and return the required values.
            _averageTimeForMessage = AverageTimeForMessage(message);
            _averageLettersPerMessage = AverageLettersPerMessage();
            _averageLettersPerUser = new DynamicJsonObject(AverageLettersPerUser());
            _messagesPerHourChart = MessagesPerHourChart();
            _lettersPerHourChart = LetterPerHourChart();
        }

        private string AverageTimeForMessage()
        {
            if (MessagesList.Count == 0)
                return "0 seconds";
            var times = new List<TimeSpan>();
            foreach (MessageModel message in MessagesList)
            {
                times.Add(new TimeSpan(message.Time.Ticks));
            }
            TimeSpan min = times.Min();
            for (int i = 0; i < times.Count; i++)
            {
                times[i] -= min;
            }
            double ticks = times.Average(x => x.Ticks);
            long longAvg = Convert.ToInt64(ticks);
            TimeSpan avg = new TimeSpan(longAvg);
            if (avg.Days >= 1)
                return avg.ToString(@"dd\.hh\:mm\:ss") + " days";
            if (avg.Hours >= 1)
                return avg.ToString(@"hh\:mm\:ss") + " hours";
            if (avg.Minutes >= 1)
                return avg.ToString(@"mm\:ss") + " minutes";
            return avg.Seconds + " seconds";
        }

        // Same method with better time-complexity - O(1).
        private string AverageTimeForMessage(MessageModel message)
        {
            if (MessagesList.Count == 0)
                return "0 seconds";
            if (message != null)
            {
                TimeSpan newMessageTimeSpan = new TimeSpan(message.Time.Ticks);
                if (MessagesList.Count == 1)
                {
                    _firstMessageTimeSpan = newMessageTimeSpan;
                    _sumMessagesTimeSum = 0;
                }
                else
                {
                    _sumMessagesTimeSum += newMessageTimeSpan.Add(_firstMessageTimeSpan.Negate()).Ticks;
                }
            }
            double avgTicks = _sumMessagesTimeSum / MessagesList.Count;
            TimeSpan avg = new TimeSpan(Convert.ToInt64(avgTicks));
            if (avg.Days >= 1)
                return avg.ToString(@"dd\.hh\:mm\:ss") + " days";
            if (avg.Hours >= 1)
                return avg.ToString(@"hh\:mm\:ss") + " hours";
            if (avg.Minutes >= 1)
                return avg.ToString(@"mm\:ss") + " minutes";
            return avg.Seconds + " seconds";
        }

        private static double AverageLettersPerMessage()
        {
            if (MessagesList.Count == 0)
                return 0;
            double avg = MessagesList.Sum(message => message.Text.Length);
            return Math.Round(avg / MessagesList.Count, 2);
        }

        private static IDictionary<string, object> AverageLettersPerUser()
        {
            IDictionary<string, object> avg = new Dictionary<string, object>();
            foreach (var user in HomeController.Users)
            {
                avg.Add(user.Name, user.AverageLattersPerMessage);
            }
            return avg;
        }

        private static int[] MessagesPerHourChart()
        {
            int[] messagesPerHour = new int[168];
            TimeSpan week = TimeSpan.FromHours(168);
            foreach (MessageModel message in MessagesList)
            {
                if (DateTime.Now.Subtract(message.Time) < week)
                {
                    messagesPerHour[167 - DateTime.Now.Subtract(message.Time).Hours]++;
                }
            }
            return messagesPerHour;
        }

        private static int[] LetterPerHourChart()
        {
            int[] lettersPerHour = new int[168];
            TimeSpan week = TimeSpan.FromHours(168);
            foreach (MessageModel message in MessagesList)
            {
                if (DateTime.Now.Subtract(message.Time) < week)
                {
                    // Possible to use regular expression here to get only specific letters. (e.g. without spaces)
                    lettersPerHour[167 - DateTime.Now.Subtract(message.Time).Hours] += message.Text.Length;
                }
            }
            return lettersPerHour;
        }
    }
}