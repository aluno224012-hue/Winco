namespace Winco.Core.Rules;

// O que uma regra muda numa janela. null = "não alterar".
// Opacity vai de 20 a 100 (percentagem).
public record WindowStyle(
    CornerPreference? Corners = null,
    RgbColor? BorderColor = null,
    RgbColor? CaptionColor = null,
    RgbColor? CaptionTextColor = null,
    bool? DarkTitleBar = null,
    BackdropKind? Backdrop = null,
    int? Opacity = null);
