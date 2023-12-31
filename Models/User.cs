using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;


namespace Fidge.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string username { get; set; } = null!;
    [BsonElement("coord")]
    public double[] coordinates { get; set; }
    public string ipAddress { get; set; }
    public string avatarUrl { get; set; }
    public string walletAddress { get; set; }
    public string expoToken { get; set; }
}