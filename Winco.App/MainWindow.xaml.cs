using System.Windows;
using System.Windows.Controls;
using Winco.Core.Colors;
using Winco.Core.Windows;
using Winco.Platform;
 
namespace Winco.App;
 
/// Interação lógica para MainWindow.xaml
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadWindows();
    }
 
    // A janela que está selecionada na lista, ou null se não houver nenhuma.
    // "as WindowInfo" devolve null em vez de dar erro se não for um WindowInfo.
    private WindowInfo? SelectedWindow => WindowsList.SelectedItem as WindowInfo;
 
    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        LoadWindows();
    }
 
    private void LoadWindows()
    {
        // Trocar a lista apaga a seleção, por isso o SelectionChanged
        // corre sozinho e desativa os botões.
        WindowsList.ItemsSource = Wincoenumerador.GetTopLevelWindows();
    }
 
    // Corre sempre que o utilizador escolhe (ou deixa de ter) uma janela na lista.
    private void WindowsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        WindowInfo? window = SelectedWindow;
 
        StylePanel.IsEnabled = window is not null;
        StatusText.Text = window is null
            ? "Nenhuma janela selecionada."
            : $"Selecionada: {window.ProcessName} — {window.Title}";
    }
 
    // Todos os botões fazem o mesmo: pegam na janela selecionada, chamam um
    // método do DwmStyler com o Handle dela e mostram se correu bem.
    // "action" é o método a chamar: recebe o Handle (nint) e devolve bool.
    private void ApplyToSelected(string description, Func<nint, bool> action)
    {
        if (SelectedWindow is not WindowInfo window)
        {
            StatusText.Text = "Seleciona primeiro uma janela.";
            return;
        }
 
        bool ok = action(window.Handle);
 
        StatusText.Text = ok
            ? $"{description}: aplicado a {window.ProcessName}."
            : $"{description}: o Windows recusou. A janela pode já ter fechado (carrega em Atualizar) ou ser de uma app aberta como administrador.";
    }
 
    private void Modo_Escuro_Click(object sender, RoutedEventArgs e)
    {
        ApplyToSelected("Modo escuro", handle => DwmStyler.SetDarkTitleBar(handle, true));
    }
 
    private void Cantos_Retos_Click(object sender, RoutedEventArgs e)
    {
        ApplyToSelected("Cantos retos", handle => DwmStyler.SetCorners(handle, CornerStyle.Square));
    }
 
    private void Borda_Vermelha_Click(object sender, RoutedEventArgs e)
    {
        uint red = ColorConversion.HexToColorRef("#FF0000");
        ApplyToSelected("Borda vermelha", handle => DwmStyler.SetBorderColor(handle, red));
    }
 
    private void Barra_Azul_Click(object sender, RoutedEventArgs e)
    {
        uint blue = ColorConversion.HexToColorRef("#0078D4");
        ApplyToSelected("Barra azul", handle => DwmStyler.SetCaptionColor(handle, blue));
    }
 
    // Volta tudo ao que o Windows usa por omissão.
    // 0xFFFFFFFF = "cor por omissão" para as cores do DWM.
    private void Repor_Click(object sender, RoutedEventArgs e)
    {
        const uint DefaultColor = 0xFFFFFFFF;
 
        ApplyToSelected("Repor", handle =>
            DwmStyler.SetDarkTitleBar(handle, false)
            & DwmStyler.SetCorners(handle, CornerStyle.Default)
            & DwmStyler.SetBorderColor(handle, DefaultColor)
            & DwmStyler.SetCaptionColor(handle, DefaultColor));
    }
}
 