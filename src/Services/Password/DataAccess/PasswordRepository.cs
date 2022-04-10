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

    public void Delete(string userId, string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id) & Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        Collection.DeleteOne(filter);
    }

    public En.Password Get(string userId, string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id) & Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var password = Collection.Find(filter).FirstOrDefault();
        return password;
    }

    public List<En.Password> Get(string userId, int skip, int take)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var projection = Builders<En.Password>.Projection
        .Include(x => x.Title)
        .Include(x => x.Username)
        .Include(x => x.Detail)
        .Include(x => x.LastUpdateDate)
        .Include(x => x.CreationDate)
        .Include(x => x.OwnerId);
        var passwords = Collection.Find(filter).Project<En.Password>(projection).Skip(skip).Limit(take).ToList();

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

    public async Task<long> GetUserPasswordsCount(string userId)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var count = await Collection.CountDocumentsAsync(filter);
        return count;
    }
}