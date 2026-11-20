namespace Winco.Core.Rules;

public record Rule(Guid Id, string Name, bool Enabled, MatchCriteria Match, WindowStyle Style);
