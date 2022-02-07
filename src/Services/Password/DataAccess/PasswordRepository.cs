using MongoDB.Driver;
using En = Password.Entities;

public class PasswordRepository : IPasswordRepository, IMongoDbRepository<En.Password>
{
    public IMongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<En.Password> Collection { get; set; }

    public PasswordRepository(IMongoClient mongoClient)
    {
        Client = mongoClient;
        Database = Client.GetDatabase("OnePassMini");
        Collection = Database.GetCollection<En.Password>("Passwords");
    }

    public void Delete(string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id);
        Collection.DeleteOne(filter);
    }

    public En.Password Get(string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id);
        var password = Collection.Find(filter).FirstOrDefault();
        return password;
    }

    public List<En.Password> Get()
    {
        var filter = Builders<En.Password>.Filter.Empty;
        var projection = Builders<En.Password>.Projection
        .Include(x => x.Title)
        .Include(x => x.Username)
        .Include(x => x.Detail)
        .Include(x => x.LastUpdateDate)
        .Include(x => x.CreationDate);
        var passwords = Collection.Find(filter).Project<En.Password>(projection).ToList();

        return passwords;
    }

    public void Insert(En.Password password)
    {
        Collection.InsertOne(password);
    }

    public void Update(En.Password password)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, password.Id);
        Collection.ReplaceOne(filter, password);
    }
}