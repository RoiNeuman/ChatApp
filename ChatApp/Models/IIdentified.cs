using MongoDB.Bson;

namespace ChatApp.Models
{
    public interface IIdentified
    {
        ObjectId Id { get; }
    }
}
