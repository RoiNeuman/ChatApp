using System;
using System.Threading;
using ChatApp.Models;
using Microsoft.AspNet.SignalR;

namespace ChatApp.Hubs.Charts
{
    public class ChartDataUpdate
    {
        // Singleton instance
        private static readonly Lazy<ChartDataUpdate> _instance = new Lazy<ChartDataUpdate>(() => new ChartDataUpdate());
        public static ChartDataUpdate Instance => _instance.Value;
        // Send data every hour
        private readonly int _updateInterval = 60*60*1000;
        private Timer _timer;
        private volatile bool _sendingChartData;
        private readonly object _chartUpateLock;
        private readonly StatisticsModel _statistics;


        private ChartDataUpdate()
        {
            _statistics = new StatisticsModel();
            _chartUpateLock = new object();
        }

        // Calling this method starts the Timer    
        public void GetChartData()
        {
            _timer = new Timer(ChartTimerCallBack, null, _updateInterval, _updateInterval);

        }
        private void ChartTimerCallBack(object state)
        {
            if (_sendingChartData) return;
            lock (_chartUpateLock)
            {
                if (_sendingChartData) return;
                _sendingChartData = true;
                SendChartData();
                _sendingChartData = false;
            }
        }

        private void SendChartData()
        {
            _statistics.Init(null);
            GetAllClients().All.UpdateChart(_statistics);

        }

        private static dynamic GetAllClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients;
        }
    }
}