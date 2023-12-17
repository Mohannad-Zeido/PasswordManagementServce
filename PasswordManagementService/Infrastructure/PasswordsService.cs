using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PasswordManagementService.Infrastructure;

public class PasswordsService
{
    private readonly IMongoCollection<PasswordStorage> _booksCollection;

    public PasswordsService(
        IOptions<PMSDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<PasswordStorage>(
            bookStoreDatabaseSettings.Value.PasswordsCollectionName);
    }

    public async Task<List<PasswordStorage>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<PasswordStorage?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(PasswordStorage newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, PasswordStorage updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}