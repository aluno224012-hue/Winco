using Winco.Core.Rules;

// Testes do RgbColor (Winco.Core/Rules/RgbColor.cs).
public class TestesRgbColor
{
    [Fact]
    public void ToString_DevolveMaiusculas()
    {
        Assert.Equal("#FF8000", RgbColor.Parse("#ff8000").ToString());
    }

    [Theory]
    [InlineData("#FF0000", 0x000000FFu)] // vermelho
    [InlineData("#00FF00", 0x0000FF00u)] // verde
    [InlineData("#0000FF", 0x00FF0000u)] // azul
    [InlineData("#123456", 0x00563412u)] // troca dos bytes
    public void ToColorRef_TrocaVermelhoComAzul(string hex, uint esperado)
    {
        Assert.Equal(esperado, RgbColor.Parse(hex).ToColorRef());
    }

    [Theory]
    [InlineData("FF0000")]   // falta o #
    [InlineData("#GG0000")]  // não é hexadecimal
    [InlineData("#FF00")]    // curto demais
    [InlineData(" #FF000")]  // espaço
    [InlineData("")]
    public void TextoInvalido_ParseLancaFormatException(string texto)
    {
        Assert.Throws<FormatException>(() => RgbColor.Parse(texto));
    }

    [Theory]
    [InlineData("FF0000")]
    [InlineData("#GG0000")]
    [InlineData("")]
    [InlineData(null)]
    public void TextoInvalido_TryParseDevolveFalse(string? texto)
    {
        Assert.False(RgbColor.TryParse(texto, out _));
    }
}
