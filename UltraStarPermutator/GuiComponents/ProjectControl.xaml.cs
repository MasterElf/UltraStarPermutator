using System.Windows.Controls;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public ProjectControl()
        {
            InitializeComponent();
        }

        private void AddPartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ProjectModel projectModel)
            {
                projectModel.Parts.Add(new PartModel());
            }
        }
    }
}