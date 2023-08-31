using System.ComponentModel.DataAnnotations;

namespace FileShare.Configuration;

public class MongoSettings
{
    public const string CONFIG_SECTION_NAME = "MongoSettings";

    [Required]
    public string ConnectionString { get; set; } = null!;
    [Required]
    public string Database { get; set; } = null!;
}
