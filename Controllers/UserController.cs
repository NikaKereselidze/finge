using System;
using Microsoft.AspNetCore.Mvc;
using Fidge.Services;
using Fidge.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Fidge.Controllers;


[Controller]
[Route("api/[controller]")]
public class UserController: Controller
{
    
    private readonly MongoDBService _mongoDBService;
    
    
    public UserController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        await _mongoDBService.CreateAsync(user);
        return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
    }
    
    [HttpGet]
    public async Task<List<User>> Get([FromQuery] double lng, [FromQuery] double lat) {
        var builder = Builders<User>.Filter;
        var point = GeoJson.Point(GeoJson.Position(lng, lat));
        
        var filter = builder.GeoWithinCenter(x => x.coordinates, point.Coordinates.X, point.Coordinates.Y, 10);
        Console.WriteLine($"{filter} example:");
        
        var serializerRegistry = BsonSerializer.SerializerRegistry;
        var documentSerializer = serializerRegistry.GetSerializer<User>();
        
        var rendered = filter.Render(documentSerializer, serializerRegistry);
        Console.WriteLine($"{rendered} example:");
        
        return await _mongoDBService.GetAsync(rendered);
    }
}