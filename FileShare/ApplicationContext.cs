using FileShare.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileShare;

public class ApplicationContext
{
    private readonly IMongoDatabase _database;
    private readonly IGridFSBucket _bucket;

    public ApplicationContext(IOptions<MongoSettings> connectionOptions)
    {
        var connection = connectionOptions.Value;
        var mongoUrl = new MongoUrl($"{connection.ConnectionString}/{connection.Database}");
        var client = new MongoClient(mongoUrl);
        if (client != null)
        {
            _database = client.GetDatabase(mongoUrl.DatabaseName);
            _bucket = new GridFSBucket(_database);
        }
        else
        {
            throw new Exception("Database connection not possible"); // TODO: refactor this
        }
    }

    public IGridFSBucket Bucket => _bucket;

    public async Task<GridFSUploadStream> OpenUploadStreamAsync(string fileName) => await _bucket.OpenUploadStreamAsync(fileName);
}
