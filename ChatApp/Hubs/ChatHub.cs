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
        private readonly StatisticsModel _statistics = new StatisticsModel();


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
                    _statistics.Init(newMessage);
                    Clients.All.changeStatistics(_statistics);
                }
            }
        }

        public void InitChartData()
        {
            //Show Chart initially when InitChartData called first time
            Clients.All.UpdateStatistics(_statistics);

            // Call GetChartData to send Chart data every 1 hour    
            _chartInstance.GetChartData();
        }
    }
}