namespace Winco.Core.Rules;

// Os mesmos valores que o DWM usa (DWM_SYSTEMBACKDROP_TYPE).
// Só funciona no Windows 11 22H2 (build 22621) ou mais recente.
public enum BackdropKind
{
    Auto = 0,
    None = 1,
    Mica = 2,
    Acrylic = 3,
    MicaAlt = 4,
}
