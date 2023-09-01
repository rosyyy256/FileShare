# FileShare

.NET 6 file sharing application. App allows to upload, store and download files.

User can upload multiple files at once with upload status of each in real-time. User can view the list if uploaded files, download, delete them and create a one-time link for a single file.

One-time link allows to download a file. After file is downloaded, the link will no longer work.

Application is written on Blazor Server. MubBlazor lib is used for UI. Files are stored in MongoDB via GridFS.

## Configuration

|Name                           |Type                   |Description
|:----                          |:----                  |:----
|MongoSettings:ConnectionString |`mongo://[HOST]:[PORT]`|Mongo URI
|MongoSettings:Database         |`<string>`             |Mongo database name
|MaxFilesCount                  |`<int>`                |Max files count allowed for a single upload. Default - 10
|MaxFileSize                    |`<long>`               |Max size for a single file in bytes. Defalut - 104,857,600 (100MB)

## Local launch

```bash
$ docker compose up
```