using System.Collections.Generic;
using ChatApp.Controllers;
using MongoDB.Driver;

namespace ChatApp.Models
{
    public static class Database
    {
        private const string ConnectionString = "mongodb://localhost";

        private static IMongoClient _client;
        private static IMongoDatabase _database;
        public  static IMongoCollection<MessageModel> MessagesCollection { get; set; }


        private static void CreateConnection()
        {
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase("ChatAppDB");
            MessagesCollection = _database.GetCollection<MessageModel>("Messages");
        }

        // To save item (_item) in a collection (_col) use: _col.SaveAsync(_item);
        public static void SaveAsync<T>(this IMongoCollection<T> collection, T entity)
            where T : IIdentified
        {
            if (collection == null)
            {
                CreateConnection();
            }
            collection?.InsertOneAsync(entity);
        }
        
        public static IList<MessageModel> GetMessagesList()
        {
            if (MessagesCollection == null)
            {
                CreateConnection();
            }
            List<MessageModel> messages = MessagesCollection.Find(_ => true).ToList();
            messages.Sort();
            foreach (MessageModel message in messages)
            {
                if (HomeController.UserDictionary.ContainsKey(message.Author))
                {
                    HomeController.UserDictionary[message.Author].Messages.Add(message);
                }
                else
                {
                    UserModel newUser = new UserModel(message.Author);
                    newUser.Messages.Add(message);
                    newUser.GetAverageLattersPerMessage();
                    HomeController.Users.Add(newUser);
                    HomeController.UserDictionary.Add(message.Author, newUser);
                }
            }
            StatisticsModel.FirstMessageIndex = messages.Count + 1;
            return messages;
        }
    }
}