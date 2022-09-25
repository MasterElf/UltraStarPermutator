using System.Windows;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProjectModel projectModel = new ProjectModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = projectModel;
        }
    }
}