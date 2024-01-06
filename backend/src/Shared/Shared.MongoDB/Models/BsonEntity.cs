using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.MongoDB.Models;

public class BsonEntity
{
    [BsonId] public ObjectId Id { get; set; }
}