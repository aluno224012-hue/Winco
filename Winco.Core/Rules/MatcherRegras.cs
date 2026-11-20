using Winco.Core.Windows;

namespace Winco.Core.Rules;

public static class RuleMatcher
{
    // Devolve a primeira regra ativa que corresponde à janela, ou null se nenhuma corresponder.
    // A ordem da lista conta: a primeira que serve ganha.
    public static Rule? FindMatch(IReadOnlyList<Rule> rules, WindowInfo window)
    {
        foreach (Rule rule in rules)
        {
            if (rule.Enabled && Matches(rule.Match, window))
            {
                return rule;
            }
        }

        return null;
    }

    private static bool Matches(MatchCriteria match, WindowInfo window)
    {
        // Nomes e caminhos de ficheiros no Windows não distinguem maiúsculas.
        // O ProcessName é obrigatório, por isso compara-se sempre (vazio não serve para tudo).
        if (!string.Equals(match.ProcessName, window.ProcessName, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(match.ProcessPath)
            && !string.Equals(match.ProcessPath, window.ProcessPath, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // A classe da janela tem de ser exatamente igual.
        if (!string.IsNullOrEmpty(match.WindowClass)
            && !string.Equals(match.WindowClass, window.ClassName, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(match.TitleContains)
            && !window.Title.Contains(match.TitleContains, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
