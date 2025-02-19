using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace common.Entities
{
    public abstract class AuditEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID tự động sinh của MongoDB

        [BsonElement("created_at")]
        [BsonRepresentation(BsonType.Int64)]
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        [BsonElement("updated_at")]
        [BsonRepresentation(BsonType.Int64)]
        public long UpdatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        [BsonElement("created_by")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; }

        [BsonElement("updated_by")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UpdatedBy { get; set; }
    }
}

