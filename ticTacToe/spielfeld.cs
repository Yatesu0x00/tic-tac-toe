using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ticTacToe
{
    class spielfeld
    {       
        Rectangle rect = new Rectangle();

        private bool wasClicked = false;
             
        //Eigenschaften
        public Rectangle Rect
        {
            get
            {
                return rect;
            }
        }    

        public bool WasClicked
        {
            get
            {
                return wasClicked;
            }

            set
            {
                wasClicked = value;
            }
        }

        public int owner { get; set; }

        public Brush sp1 { get; set; }
        public Brush sp2 { get; set; }

        //Konstruktor
        public spielfeld(double _width, double _height, double i, double j, MainWindow win)
        {
            rect.Width = _width;
            rect.Height = _height;

            rect.Fill = Brushes.GhostWhite;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1;
           
            Canvas.SetLeft(rect, (i));
            Canvas.SetTop(rect, (j));
        }

        public void draw(Canvas c)
        {
            if (!c.Children.Contains(rect))
            {
                c.Children.Add(rect);
            }
        }

        public void resize(double sx, double sy)
        {
            rect.Width = rect.Width * sx;
            rect.Height = rect.Height * sy;

            Canvas.SetLeft(rect, sx * Canvas.GetLeft(rect));
            Canvas.SetTop(rect, sy * Canvas.GetTop(rect));
        }
    }
}
