using FileShare.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileShare;

public class FilesContext
{
    private readonly IMongoDatabase _database;
    private readonly IGridFSBucket _bucket;

    public FilesContext(IOptions<MongoSettings> connectionOptions)
    {
        var connection = connectionOptions.Value;
        var mongoUrl = new MongoUrl($"{connection.ConnectionString}/{connection.Database}");
        var client = new MongoClient(mongoUrl);
        _database = client.GetDatabase(mongoUrl.DatabaseName);
        _bucket = new GridFSBucket(_database);
    }

    public async Task<List<GridFSFileInfo>> GetFilesInfoAsync()
    {
        return (await _bucket.FindAsync("{}")).ToList();
    }

    public async Task<GridFSFileInfo> GetFileInfoAsync(ObjectId id)
    {
        return await FindByIdAsync(id).FirstAsync();
    }

    public async Task<bool> FileExistsAsync(ObjectId id)
    {
        return await FindByIdAsync(id).AnyAsync();
    }

    public async Task DeleteFileAsync(ObjectId fileId)
    {
        try
        {
            await _bucket.DeleteAsync(fileId);
        }
        catch { }
    }

    public async Task<GridFSUploadStream> OpenUploadStreamAsync(string fileName) => await _bucket.OpenUploadStreamAsync(fileName);

    public async Task<GridFSDownloadStream<ObjectId>> OpenDownloadStreamAsync(ObjectId id) => await _bucket.OpenDownloadStreamAsync(id);

    private IAsyncCursor<GridFSFileInfo> FindByIdAsync(ObjectId id)
    {
        var filter = Builders<GridFSFileInfo>.Filter.And(
            Builders<GridFSFileInfo>.Filter.Eq("_id", id));
        var options = new GridFSFindOptions
        {
            Limit = 1
        };

        return _bucket.Find(filter, options);
    }
}
