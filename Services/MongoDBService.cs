using Fidge.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Fidge.Services;

public class MongoDBService {
    public readonly IMongoCollection<User> _userCollection;
    

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) 
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _userCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<bool> CreateIndexAsync(IndexKeysDefinition<User> keys, CreateIndexOptions indexOptions)
    {
        await _userCollection.Indexes.CreateOneAsync(keys, indexOptions);
        return true;

    }

    public async Task<List<User>> GetAsync(BsonDocument bsonDocument)
    {
        return await _userCollection.Find(bsonDocument).ToListAsync();
    }
    
    public async Task CreateAsync(User user)
    {
        // var filter = Builders<User>.Filter;
        //
        // var filterDefinition = filter.And(
        //     filter.Eq("_id", user.Id)
        // );
        //
        // var userData = await _userCollection.Find(filterDefinition).Limit(1).SingleAsync();
        //
        // if (userData is not null)
        // {
        //     
        //     var updateFilter = Builders<User>.Filter
        //         .Eq(user => user.Id, user);
        //     var update = Builders<Restaurant>.Update
        //         .Set(restaurant => restaurant.Name, newValue);
        //     
        //     await _userCollection.UpdateOneAsync()
        // }
        
        await _userCollection.InsertOneAsync(user);
        return;
    }

    public async Task<UpdateResult> UpdateCoords(string userId, double[] coordinates)
    {
        
        var fetchFilter = Builders<User>.Filter.Eq("Id", userId);
        
        var UserCords = await _userCollection.Find(fetchFilter).ToListAsync();
        
        Console.WriteLine($"{UserCords}");
        
        var filter = Builders<User>.Filter
            .Eq(user => user.Id, "add");
        var update = Builders<User>.Update
            .Set(user => user.coordinates, coordinates);
        return await _userCollection.UpdateOneAsync(filter, update);
    }

}