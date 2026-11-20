namespace Winco.Core.Rules;

// A que janelas uma regra se aplica. Só o ProcessName é obrigatório;
// os outros campos, se ficarem null ou vazios, não são verificados.
public record MatchCriteria(
    string ProcessName,
    string? ProcessPath = null,
    string? WindowClass = null,
    string? TitleContains = null);
