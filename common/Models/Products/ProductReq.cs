using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace common.Models.Products
{
	public class ProductReq
    {
        public ProductReq(string code)
        {
            this.Code = code;
        }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Ratings { get; set; }

        [BsonElement("views")]
        public int Views { get; set; }

        [BsonElement("type")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Type { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Status { get; set; }

        [BsonElement("img")]
        public List<string> Img { get; set; }

        [BsonElement("hmac")]
        public string Hmac { get; set; }

        [BsonElement("company_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CompanyId { get; set; }

        [BsonElement("link")]
        public List<string> Link { get; set; }

        [BsonElement("path")]
        public List<string> Path { get; set; }
    }
}

