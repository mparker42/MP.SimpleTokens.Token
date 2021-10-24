using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MP.SimpleTokens.Token.Models
{
    [BsonIgnoreExtraElements]
    public class TokenInfo
    {
        public string Name { get; set; }

        [Url]
        public string Url { get; set; }
    }
}
