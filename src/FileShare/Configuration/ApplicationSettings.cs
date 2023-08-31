﻿namespace FileShare.Configuration;

public class ApplicationSettings
{
    public long MaxFileSize { get; set; } = 1024 * 1024 * 100;
    public int MaxFilesCountPerInput { get; set; } = 10;
}
