using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace common.Models.Products
{
	public class ProductDTO
	{
		public ProductDTO()
		{
		}

        public string Id { get; set; }

        public long CreatedAt { get; set; }

        public long UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Ratings { get; set; }

        public int Views { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public List<string> Img { get; set; }

        public string Hmac { get; set; }

        public string CompanyId { get; set; }

        public List<string> Link { get; set; }

        public List<string> Path { get; set; }
    }
}

