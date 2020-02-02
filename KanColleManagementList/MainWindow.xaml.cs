using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KanColleManagementList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataInfo KanColleData = new DataInfo();
        public MainWindow()
        {
            InitializeComponent();
            KanColleDataGrid.DataContext = KanColleData.KanColleData;
            //KanColleDataGrid.DataContext = KanColleData.getKanColleDatable();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KanColleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Click_CsvImport(object sender, SelectionChangedEventArgs e) 
        {

        }

        private void Click_Data(object sender, SelectionChangedEventArgs e) 
        {
        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CsvImport_Click(object sender, RoutedEventArgs e)
        {
            KanColleData.UpdateViewDataTable();
        }
    }
}
