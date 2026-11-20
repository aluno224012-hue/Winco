using Winco.Core.Rules;
using Winco.Core.Windows;

// Testes do RuleMatcher.FindMatch (Winco.Core/Rules/MatcherRegras.cs).
public class TestesMatcherRegras
{
    // Uma janela do Bloco de Notas, usada em quase todos os testes.
    private static readonly WindowInfo BlocoDeNotas = new(
        Handle: 1,
        Title: "Sem título - Bloco de Notas",
        ClassName: "Notepad",
        ProcessId: 100,
        ProcessName: "notepad.exe",
        ProcessPath: @"C:\Windows\System32\notepad.exe");

    // Cria uma regra só com o que interessa ao teste; o estilo fica vazio ("não alterar").
    private static Rule Regra(string nome, MatchCriteria match, bool enabled = true) =>
        new(Guid.NewGuid(), nome, enabled, match, new WindowStyle());

    [Fact]
    public void ListaVazia_DevolveNull()
    {
        Assert.Null(RuleMatcher.FindMatch([], BlocoDeNotas));
    }

    [Fact]
    public void NenhumaRegraCorresponde_DevolveNull()
    {
        var regras = new[] { Regra("Paint", new MatchCriteria("mspaint.exe")) };

        Assert.Null(RuleMatcher.FindMatch(regras, BlocoDeNotas));
    }

    [Fact]
    public void ProcessName_Igual_Corresponde()
    {
        var regra = Regra("Bloco", new MatchCriteria("notepad.exe"));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void ProcessName_IgnoraMaiusculas()
    {
        var regra = Regra("Bloco", new MatchCriteria("NOTEPAD.EXE"));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void ProcessName_Vazio_NaoServeParaTodas()
    {
        var regra = Regra("Sem processo", new MatchCriteria(""));

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void RegraDesligada_EIgnorada()
    {
        var regra = Regra("Desligada", new MatchCriteria("notepad.exe"), enabled: false);

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void RegraDesligada_PassaParaASeguinte()
    {
        var desligada = Regra("Desligada", new MatchCriteria("notepad.exe"), enabled: false);
        var ligada = Regra("Ligada", new MatchCriteria("notepad.exe"));

        Assert.Same(ligada, RuleMatcher.FindMatch([desligada, ligada], BlocoDeNotas));
    }

    [Fact]
    public void VariasCorrespondem_GanhaAPrimeira()
    {
        var primeira = Regra("Primeira", new MatchCriteria("notepad.exe"));
        var segunda = Regra("Segunda", new MatchCriteria("notepad.exe", TitleContains: "Bloco"));

        Assert.Same(primeira, RuleMatcher.FindMatch([primeira, segunda], BlocoDeNotas));
    }

    [Fact]
    public void ProcessPath_IgnoraMaiusculas()
    {
        var regra = Regra("Caminho", new MatchCriteria("notepad.exe", ProcessPath: @"c:\windows\system32\NOTEPAD.EXE"));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void ProcessPath_Diferente_NaoCorresponde()
    {
        var regra = Regra("Outro caminho", new MatchCriteria("notepad.exe", ProcessPath: @"C:\Apps\notepad.exe"));

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    // Janelas de apps abertas como administrador não têm caminho (ProcessPath = null).
    [Fact]
    public void ProcessPath_JanelaSemCaminho_NaoCorresponde()
    {
        var janelaAdmin = BlocoDeNotas with { ProcessPath = null };
        var regra = Regra("Caminho", new MatchCriteria("notepad.exe", ProcessPath: @"C:\Windows\System32\notepad.exe"));

        Assert.Null(RuleMatcher.FindMatch([regra], janelaAdmin));
    }

    [Fact]
    public void WindowClass_Igual_Corresponde()
    {
        var regra = Regra("Classe", new MatchCriteria("notepad.exe", WindowClass: "Notepad"));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void WindowClass_DistingueMaiusculas()
    {
        var regra = Regra("Classe", new MatchCriteria("notepad.exe", WindowClass: "notepad"));

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Theory]
    [InlineData("bloco de notas")]
    [InlineData("SEM TÍTULO")]
    [InlineData("Notas")]
    public void TitleContains_IgnoraMaiusculas(string parte)
    {
        var regra = Regra("Título", new MatchCriteria("notepad.exe", TitleContains: parte));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void TitleContains_NaoEstaNoTitulo_NaoCorresponde()
    {
        var regra = Regra("Título", new MatchCriteria("notepad.exe", TitleContains: "Paint"));

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CamposOpcionaisVazios_SaoIgnorados(string? vazio)
    {
        var regra = Regra("Só processo", new MatchCriteria("notepad.exe", vazio, vazio, vazio));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    [Fact]
    public void TodosOsCamposPreenchidos_Correspondem()
    {
        var regra = Regra("Completa", new MatchCriteria(
            "notepad.exe",
            ProcessPath: @"C:\Windows\System32\notepad.exe",
            WindowClass: "Notepad",
            TitleContains: "Bloco"));

        Assert.Same(regra, RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }

    // Basta um campo não corresponder para a regra inteira falhar.
    [Fact]
    public void UmCampoFalha_RegraNaoCorresponde()
    {
        var regra = Regra("Quase", new MatchCriteria(
            "notepad.exe",
            ProcessPath: @"C:\Windows\System32\notepad.exe",
            WindowClass: "Notepad",
            TitleContains: "Excel"));

        Assert.Null(RuleMatcher.FindMatch([regra], BlocoDeNotas));
    }
}
