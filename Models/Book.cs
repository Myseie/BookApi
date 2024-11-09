using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace BookApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime PublicationDate { get; set; }
    }
}
