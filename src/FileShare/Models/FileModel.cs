using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace FileShare.Models;

public class FileModel
{
    public ObjectId Id { get; init; }
    public string Name { get; init; }
    public DateTime UploadedAt { get; init; }

    public FileModel(GridFSFileInfo fileInfo)
    {
        Id = fileInfo.Id;
        Name = fileInfo.Filename;
        UploadedAt = fileInfo.UploadDateTime;
    }
}
