public class UserRepository : IUserRepository, IMongoDbRepository<User>
{
    public IMongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<User> Collection { get; set; }

    public UserRepository(IMongoClient mongoClient)
    {
        Client = mongoClient;
        Database = Client.GetDatabase("TheHost");
        Collection = Database.GetCollection<User>("Users");
    }

    //TODO: password will be decrypted before return
    public User GetByUsername(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        var user = Collection.Find(filter).FirstOrDefault();
        return user;
    }
}