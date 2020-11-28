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
    /// Interaktionslogik für dialogWindow.xaml
    /// </summary>
    public partial class dialogWindow : Window
    {
        public BrushConverter kokosnuss = new BrushConverter();

        public string spieler1 { get; set; }
        public string spieler2 { get; set; }
        public string fb1 { get; set; }
        public string fb2 { get; set; }
        public string fb3 { get; set; }

        public int feld_groesse { get; set; }
        public int timer { get; set; }

        public dialogWindow()
        {                    
            InitializeComponent();
        }

        //Eventhandler für OK Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                spieler1 = tb_sp1.Text;
                spieler2 = tb_sp2.Text;                

                feld_groesse = Convert.ToInt16(UDC_anzFeld.Text);
                timer = Convert.ToInt16(UDC_time.Text);

                fb1 = cp_sp1.SelectedColor.ToString();
                fb2 = cp_sp2.SelectedColor.ToString();
                fb3 = cp_fb3.SelectedColor.ToString();
                
                if(feld_groesse < 3 || feld_groesse > 12) //Abfrage für die minimale und maximale Feldgröße
                {
                    throw new Exception("Die Anzahl an Spalten und Zeilen muss mindestens 3 und maximal 12 betragen!");
                }
                else if (fb1 == fb2 || fb1 == fb3 || fb2 == fb3) //Ob gleiche Farben abfrage
                {
                    throw new Exception("Es dürfen nicht die selben Farben verwendet werden!");
                }
                else
                {
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message, "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Eventhandler für Ende Button
        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
