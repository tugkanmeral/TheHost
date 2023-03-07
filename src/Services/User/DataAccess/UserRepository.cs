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

    public User GetById(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var user = Collection.Find(filter).FirstOrDefault();
        return user;
    }

    public IEnumerable<User> Get()
    {
        var filter = Builders<User>.Filter.Empty;
        var users = Collection.Find<User>(filter).ToList();
        return users;
    }

    public void Insert(User user)
    {
        Collection.InsertOne(user);
    }

    public void Delete(string id)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        Collection.DeleteOne(filter);
    }
}