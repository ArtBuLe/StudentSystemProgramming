namespace StudentSystemProgramming.Models;

public sealed class AppSettings
{
    public int SchemaVersion { get; set; } = 1;

    public string Theme { get; set; } = "Light";

    public int LaunchCount { get; set; }
}