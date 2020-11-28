using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ticTacToe
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        spielfeld[,] feld;
        dialogWindow dlg;
        endWindow endWin;
        DispatcherTimer timer;

        int spieler = 1;
        int anz;
        int timer1, timer2;
        
        //Konstruktor
        public MainWindow()
        {
            dlg = new dialogWindow();
            endWin = new endWindow();

            //Aufruf des Dialogfensters
            if ((bool)dlg.ShowDialog())
            {
                InitializeComponent();
            }
            else
            {
                Environment.Exit(0);
            }

            //Spielernamen von Dialogfenster werden übernommen
            lb_sp1.Content = dlg.spieler1;
            lb_sp2.Content = dlg.spieler2;           
        

            timer = new DispatcherTimer();

            // Tick Intervall festlegen (d, h, m, s, ms)
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            //Evenhandler für Timer übergeben
            timer.Tick += new EventHandler(timer_tick);

            //Timer in Minuten umrechnen
            lb_timer1.Content = TimeSpan.FromSeconds(dlg.timer * 60);
            lb_timer2.Content = TimeSpan.FromSeconds(dlg.timer * 60);

            timer1 = dlg.timer * 60;
            timer2 = dlg.timer * 60;

            //Timer wird sofort von Anfang an gestarten, da es sich in Konstruktor befinden!
            timer.Start();

            //Konvertierung von strings in Brushes
            BrushConverter bc = new BrushConverter();

            lb_sp1.Foreground = (Brush)bc.ConvertFromString(dlg.fb1);
            lb_sp2.Foreground = (Brush)bc.ConvertFromString(dlg.fb2);
            lb_timer1.Foreground = (Brush)bc.ConvertFromString(dlg.fb1);
            lb_timer2.Foreground = (Brush)bc.ConvertFromString(dlg.fb2);
            c.Background = (Brush)bc.ConvertFromString(dlg.fb3);
        }

        //Eventhandler für das laden von Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {         
            feld = new spielfeld[dlg.feld_groesse, dlg.feld_groesse]; //Dynamische Feldgöße erzeugen

            //ANMERKUNG: Beim folgenen code habe ich Hilfe von Tim Bablick erhalten!
            //ab hier
            double width = 70 * 3 / dlg.feld_groesse; //standard Feld größer festlegen
            double height = width; // höhe auf breite setzten

            double mX = c.ActualWidth / 2;
            double mY = c.ActualHeight / 2;

            if(dlg.feld_groesse % 2 == 0) //grade Felder überprüfen
            {
                mX -= dlg.feld_groesse / 2 * width;
                mY -= dlg.feld_groesse / 2 * height;
            }
            else if(dlg.feld_groesse % 2 == 1) //ungrade Felder überprüfen
            {
                mX -= width / 2;
                mY -= height / 2;

                mX -= dlg.feld_groesse / 2 * width;
                mY -= dlg.feld_groesse / 2 * height;
            }
            //bis hier

            //Erzeugung des Spielfelds
            for (int i = 0; i < dlg.feld_groesse; i++)
            {
                for (int j = 0; j < dlg.feld_groesse; j++)
                {
                    feld[i, j] = new spielfeld(width, height , mX + i * width,mY + j * height, this);

                    feld[i, j].draw(c);
                }
            }
        }

        //Eventhandler für Window falls mit der linken Taste der Maus in das Feld geklickt wird
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < dlg.feld_groesse; i++)
            {
                for (int j = 0; j < dlg.feld_groesse; j++)
                {
                    if (feld[i, j].Rect.IsMouseOver)
                    {
                        if (!feld[i, j].WasClicked)
                        {
                            feld[i, j].WasClicked = true;

                            if (spieler == 1)
                            {
                                feld[i, j].Rect.Fill = lb_sp1.Foreground; //Farbe von Typ Brush übergeben -> aus DialogWindow
                                feld[i, j].owner = 1;

                                spieler = 2;

                                checkWinner();                      
                            }
                            else if (spieler == 2)
                            {
                                feld[i, j].Rect.Fill = lb_sp2.Foreground; //Farbe von Typ Brush übergeben -> aus DialogWindow
                                feld[i, j].owner = -1;

                                spieler = 1;

                                checkWinner();
                            }
                        }
                    }
                }
            }
        }
      
        //Funktion zur überprüfung wer gewonnen hat
        private void checkWinner()
        {
           int horizontal = 0, vertikal = 0;         

            anz++;

            //abfrage für horizontal und vertikal
            for (int i = 0; i < dlg.feld_groesse; i++)
            {
                for (int j = 0; j < dlg.feld_groesse; j++)
                {
                    vertikal += feld[j, i].owner;
                    horizontal += feld[i, j].owner;                                    
                }
                if (horizontal == -dlg.feld_groesse || vertikal == -dlg.feld_groesse)
                {
                    MessageBox.Show("Spieler 1 hat gewonnen!");

                    showEndWin();
                }

                else if (horizontal == dlg.feld_groesse || vertikal == dlg.feld_groesse)
                {
                    MessageBox.Show("Spieler 2 hat gewonnen!");

                    showEndWin();
                }
                
                vertikal = 0;
                horizontal = 0;
            }

            //abrage für diagonale rechts und diagonale links
            //ANMEKRUNG: Beim Folgenden Code habe ich hilfe von Klassenkamerdan erhalten!
            //ab hier
            int diagonal_l = 0, diagonal_r = 0;

            for (int i = 0; i < dlg.feld_groesse; i++)
            {
                diagonal_r += feld[i, i].owner;
                diagonal_l += feld[dlg.feld_groesse - 1 - i, i].owner;
            }          
            if (diagonal_r == -dlg.feld_groesse || diagonal_l == -dlg.feld_groesse)
            {
                MessageBox.Show("Spieler 1 hat gewonnen!");

                showEndWin();
            }
            else if (diagonal_r == dlg.feld_groesse || diagonal_l == dlg.feld_groesse)
            {
                MessageBox.Show("Spieler 2 hat gewonnen!");

                showEndWin();
            }
            //bis hier
            else if (anz == dlg.feld_groesse * dlg.feld_groesse)
            {
                MessageBox.Show("Unentschieden!");

                showEndWin();
            }
        }

        private void showEndWin()
        {
            endWin.Show();
            Close();
        }

        //Aufruf des Evenhandlers beim änderung der Canvasgröße
        private void c_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                double sx = e.NewSize.Width / e.PreviousSize.Width;
                double sy = e.NewSize.Height / e.PreviousSize.Height;

                //skalierung mit Canvas -> spielername(sp1,sp2), zeit(timer1,timer2)
                Canvas.SetTop(lb_sp1, sy * Canvas.GetTop(lb_sp1));
                Canvas.SetLeft(lb_sp1, sx * Canvas.GetLeft(lb_sp1));
                Canvas.SetTop(lb_sp2, sy * Canvas.GetTop(lb_sp2));
                Canvas.SetLeft(lb_sp2, sx * Canvas.GetLeft(lb_sp2));
                Canvas.SetTop(lb_timer1, sy * Canvas.GetTop(lb_timer1));
                Canvas.SetLeft(lb_timer1, sx * Canvas.GetLeft(lb_timer1));
                Canvas.SetTop(lb_timer2, sy * Canvas.GetTop(lb_timer2));
                Canvas.SetLeft(lb_timer2, sx * Canvas.GetLeft(lb_timer2));

                        
                for (int i = 0; i < dlg.feld_groesse; i++)
                {
                    for (int j = 0; j < dlg.feld_groesse; j++)
                    {
                        feld[i, j].resize(sx, sy); //aufrufen der resize methode
                    }
                }
            }
            catch { }
        }
       
        //Timer funktion
        private void timer_tick(object sender, EventArgs e)
        {         
            if (spieler == 1)
            {
                timer1--; 
                lb_timer1.Content = TimeSpan.FromSeconds(timer1);
            }
            else if (spieler == 2)
            {
                timer2--;
                lb_timer2.Content = TimeSpan.FromSeconds(timer2);
            }
        }

        //Menu -> neues Spiel dabei wird neues mainWindow erstellt und wir gelangen in dialogWindow
        private void neuSpiel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        //Menu-> beim drücken wird das Spiel beendet
        private void endeSpiel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
