using Winco.Core.Rules;

public class WincoConfig
{
    public int SchemaVersion { get; set; } = 1;
    public Guid ActiveProfileId { get; set; }
    public List<Profile> Profiles { get; set; } = [];
}

public class Profile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public List<Rule> Rules { get; set; } = [];
    public ThemeSettings? Theme { get; set; }
    public TaskbarSettings? Taskbar { get; set; }
    public DesktopSettings? Desktop { get; set; }
}