using Microsoft.Win32;
using StudentSystemProgramming.Models;

namespace StudentSystemProgramming.Services;

public sealed class RegistrySettingsService
{
    private const string RegistryPath = @"Software\StudentSystemProgramming";

    public AppSettings Load()
    {
        var settings = new AppSettings();

        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath);

        if (key is null)
        {
            return settings;
        }

        object? schemaVersionValue = key.GetValue("SchemaVersion");
        if (schemaVersionValue is int schemaVersion && schemaVersion > 0)
        {
            settings.SchemaVersion = schemaVersion;
        }

        object? themeValue = key.GetValue("Theme");
        object? launchCountValue = key.GetValue("LaunchCount");

        if (themeValue is string theme && (theme == "Light" || theme == "Dark"))
        {
            settings.Theme = theme;
        }

        if (launchCountValue is int launchCount && launchCount >= 0)
        {
            settings.LaunchCount = launchCount;
        }

        return settings;
    }

    public void Save(AppSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        using RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath, writable: true);

        if (settings.Theme != "Light" && settings.Theme != "Dark")
        {
            throw new ArgumentException("Недопустимая тема оформления.");
        }

        if (settings.LaunchCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(settings.LaunchCount));
        }

        key.SetValue("SchemaVersion", settings.SchemaVersion, RegistryValueKind.DWord); 

        key.SetValue("Theme", settings.Theme, RegistryValueKind.String);
        key.SetValue("LaunchCount", settings.LaunchCount, RegistryValueKind.DWord);
    }

    public void Reset()
    {
        Registry.CurrentUser.DeleteSubKeyTree(RegistryPath, throwOnMissingSubKey: false);
    }
}