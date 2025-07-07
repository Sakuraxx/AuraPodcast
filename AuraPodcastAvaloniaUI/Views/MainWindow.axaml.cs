using AuraPodcastAvaloniaUI.ViewModels;
using Avalonia.Controls;

namespace AuraPodcastAvaloniaUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}