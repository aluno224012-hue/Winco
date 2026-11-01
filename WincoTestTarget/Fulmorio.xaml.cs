using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Winco.Platform;

namespace Winco.App;


/// Interacao logica para MainWindow.xaml

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadWindows();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        LoadWindows();
    }

    private void LoadWindows()
    {
        WindowsList.ItemsSource = Wincoenumerador.GetTopLevelWindows();
    }
}
