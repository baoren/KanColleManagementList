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
        /// <summary>
        /// 艦隊一覧を格納する変数
        /// </summary>
        DataTable table = new DataTable("KabColleTable");

        /// <summary>
        /// CSVファイルの操作
        /// </summary>
        CSVInformation CSVinf = new CSVInformation();


        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists()) 
            {
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
