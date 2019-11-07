using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace WebApplication2.Models
{
    public class Match
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

       // [BsonElement("Name")]
        public string Winner { get; set; }

        public String Time { get; set; }

        public string Set1 { get; set; }

        public string Set2 { get; set; }

        public string Set3 { get; set; }

        public int Player1 { get; set; }

        public int Player2 { get; set; }

    }
}
