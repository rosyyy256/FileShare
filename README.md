# FileShare

.NET 6 file sharing application. App allows to upload, store and download files.

User can upload multiple files at once with upload status of each in real-time. User can view the list if uploaded files, download, delete them and create a one-time link for a single file.

One-time link allows to download a file. After file is downloaded, the link will no longer work.

Application is written on Blazor Server. MubBlazor lib is used for UI. Files are stored in MongoDB via GridFS.

## Configuration

```json
"MongoSettings": {
    "ConnectionString": "mongo://[HOST]:[PORT]", // mongo uri
    "Database": "database" // mongo database name
},
"MaxFilesCount": int, // max files count allowed for a single upload
"MaxFileSize": long // max size for a single file in bytes
```

## Local launch

```bash
$ docker compose up
```