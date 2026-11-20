using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Winco.Core.Rules;

// Uma cor RGB, escrita como "#RRGGBB" (é assim que fica no config.json).
// O ColorConversion continua a existir; isto é a versão "com tipo" da mesma ideia.
public readonly record struct RgbColor(byte R, byte G, byte B)
{
    // Lança FormatException se o texto não for exatamente "#RRGGBB".
    public static RgbColor Parse(string text)
    {
        if (!TryParse(text, out RgbColor color))
        {
            throw new FormatException($"Cor inválida: \"{text}\". Usa o formato #RRGGBB.");
        }

        return color;
    }

    public static bool TryParse([NotNullWhen(true)] string? text, out RgbColor color)
    {
        color = default;

        if (text is null || text.Length != 7 || text[0] != '#')
        {
            return false;
        }

        // Verificar os 6 caracteres antes, para não aceitar espaços nem sinais.
        for (int i = 1; i < 7; i++)
        {
            if (!char.IsAsciiHexDigit(text[i]))
            {
                return false;
            }
        }

        byte r = byte.Parse(text.AsSpan(1, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        byte g = byte.Parse(text.AsSpan(3, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        byte b = byte.Parse(text.AsSpan(5, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        color = new RgbColor(r, g, b);
        return true;
    }

    // Sempre em maiúsculas: "#FF8000".
    public override string ToString() => $"#{R:X2}{G:X2}{B:X2}";

    // COLORREF = 0x00BBGGRR, o formato que o DWM precisa (#FF0000 -> 0x000000FF).
    public uint ToColorRef() => ((uint)B << 16) | ((uint)G << 8) | R;
}
