using Fidge.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Fidge.Services;

public class MongoDBService {

    private readonly IMongoCollection<User> _userCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _userCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<User>> GetAsync(BsonDocument bsonDocument)
    {
        return await _userCollection.Find(bsonDocument).ToListAsync();
    }
    public async Task CreateAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
        return;
    }
    // public async Task AddToPlaylistAsync(string id, string movieId) {}
    // public async Task DeleteAsync(string id) { }

}