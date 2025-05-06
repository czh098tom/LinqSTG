using System.Collections.ObjectModel;
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

namespace LinqSTG.Demo.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //public ObservableCollection<Point> Points { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        //Points = new ObservableCollection<Point>
        //{
        //    new Point(50, 50),
        //    new Point(150, 100),
        //    new Point(200, 200),
        //};

        //this.DataContext = this;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel)?.GeneratePredictions();
    }
}