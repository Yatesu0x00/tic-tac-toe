using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ticTacToe
{
    /// <summary>
    /// Interaktionslogik für endWindow.xaml
    /// </summary>
    public partial class endWindow : Window
    {
        public endWindow()
        {
            InitializeComponent();
        }

        private void bt_j_Click(object sender, RoutedEventArgs e)
        {
                Window mw = new MainWindow();
                Close();
                mw.Show();
                
        }

        private void bt_n_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
