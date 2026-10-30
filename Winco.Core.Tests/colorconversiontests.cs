using Winco.Core.Colors;

public class ColorConversionTests
{
[Theory]
[InlineData("#FF0000", 0x000000FFu)] // vermelao
[InlineData("#00FF00", 0x0000FF00u)] // verdezinho
[InlineData("#0000FF", 0x00FF0000u)] // azulvinho
[InlineData("#123456", 0x00563412u)] // troca dos bytes
  public void HexToColorRef_TrocaVermelho(string hex, uint esperado)
  {
    Assert.Equal(esperado,ColorConversion.HexToColorRef(hex));
    
  }
}