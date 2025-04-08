using System.Windows;
namespace EightPuzzle;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        mainWindowFrame.Navigate(new Page_Main());
    }
}