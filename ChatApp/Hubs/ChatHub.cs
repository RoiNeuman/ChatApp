using System;
using ChatApp.Controllers;
using ChatApp.Hubs.Charts;
using ChatApp.Models;
using Microsoft.AspNet.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        // Create the instance of ChartDataUpdate  
        private readonly ChartDataUpdate _chartInstance;


        public ChatHub() : this(ChartDataUpdate.Instance) { }

        public ChatHub(ChartDataUpdate chartInstance)
        {
            _chartInstance = chartInstance;
        }

        public void Send(string userName, string message)
        {
            foreach (UserModel user in HomeController.Users)
            {
                if (user.Name.Equals(userName))
                {
                    MessageModel newMessage = user.AddMessage(message);
                    // Updates all the clients.
                    Clients.All.addNewMessageToPage(newMessage.Author, newMessage.Text, newMessage.Time);
                }
            }
        }

        public void InitChartData()
        {
            //Show Chart initially when InitChartData called first time    
            StatisticsModel statistics = new StatisticsModel();
            Clients.All.UpdateStatistics(statistics);

            // Call GetChartData to send Chart data every 1 hour    
            _chartInstance.GetChartData();
        }

        public void UpdateStatistics()
        {
            StatisticsModel statistics = new StatisticsModel();
            Clients.All.changeStatistics(statistics);
        }
    }
}