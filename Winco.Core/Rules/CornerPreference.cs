namespace Winco.Core.Rules;

// Os mesmos valores que o DWM usa (DWM_WINDOW_CORNER_PREFERENCE).
public enum CornerPreference
{
    Default = 0,      // o Windows decide
    Square = 1,       // sem arredondar
    Round = 2,
    RoundSmall = 3,
}
