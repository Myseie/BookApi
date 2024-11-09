using MongoDB.Driver;
using BookApi.Models;
using MongoDB.Bson;

namespace BookApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IMongoClient client)
        {
            var database = client.GetDatabase("BooksApi");
            _books = database.GetCollection<Book>("Books");
        }

        public List<Book> GetAllBooks() => 
            _books.Find(book => true).ToList();

        public Book GetBookById(string id) => 
            _books.Find(book => book.Id == id).FirstOrDefault();

        public void CreateBook(Book newBook) => 
            _books.InsertOne(newBook);

        public void UpdateBook(string id, Book updatedBook)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, id);
            _books.ReplaceOne(filter, updatedBook);
        }

        public void DeleteBook(string id) => 
            _books.DeleteOne(book => book.Id == id);

        public List<Book> GetBooksByGenre(string genre) =>
            _books.Find(book => book.Genre.ToLower() == genre.ToLower()).ToList();


        public List<Book> GetBooksSortedByDate(string sortOrder)
        {
            var sortDefinition = sortOrder.ToLower() == "desc"
                ? Builders<Book>.Sort.Descending(b => b.PublicationDate)
                : Builders<Book>.Sort.Ascending(b => b.PublicationDate);

            return _books.Find(_ => true).Sort(sortDefinition).ToList();
        }

        public List<Book> FilterBooks(string? genre, string? author)
        {
            var filterBuilder = Builders<Book>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(genre))
            {
                filter &= filterBuilder.Eq(b => b.Genre, genre);
            }

            if (!string.IsNullOrEmpty(author))
            {
                filter &= filterBuilder.Eq(b => b.Author, author);
            }

            return _books.Find(filter).ToList();
        }

        public List<Book> SearchBooks(string? title, string? author, string? genre)
        {
            var filter = Builders<Book>.Filter.Empty;
            if (!string.IsNullOrEmpty(title))
            {
                filter &= Builders<Book>.Filter.Regex("Title", new BsonRegularExpression(title,"i")); 
            }
            if (!string.IsNullOrEmpty(author))
            {
                filter &= Builders<Book>.Filter.Regex("Author", new BsonRegularExpression(author, "i"));
            }
            if (!string.IsNullOrEmpty(genre))
            {
                filter &= Builders<Book>.Filter.Eq("Genre", genre);
            }

            return _books.Find(filter).ToList();
        }
    }
}
