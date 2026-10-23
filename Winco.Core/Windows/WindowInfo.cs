namespace Winco.Core.Windows;

public record WindowInfo(nint Handle, string Title, string ClassName, uint ProcessId, string ProcessName, string? ProcessPath);