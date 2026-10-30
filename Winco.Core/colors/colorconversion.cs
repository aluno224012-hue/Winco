namespace Winco.Core.Colors;

public static class ColorConversion
{
   //#RRGGBB -> COLORREF (0x00BBGGRR), o formato que o DDWM precisa

   public static uint HexToColorRef(string hex)
   {
     uint rgb = Convert.ToUInt32(hex.TrimStart('#'), 16);
        uint r = (rgb >> 16) & 0xFF;
        uint g = (rgb >> 8) & 0xFF;
        uint b = rgb & 0xFF;

        return(b << 16) | (g << 8) | r;
   }
   
}