public interface IMongoDbRepository<T> where T : class, new()
{
    IMongoClient Client { get; set; }
    IMongoDatabase Database { get; set; }
    IMongoCollection<T> Collection { get; set; }
}