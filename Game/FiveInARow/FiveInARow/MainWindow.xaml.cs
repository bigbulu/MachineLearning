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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiveInARow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PaintGridLine();
            PaintDots();
        }

        private void PaintGridLine()
        {
            for (int i = 0; i < 15; i++)
            {
                var line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                line.X1 = 50;
                line.X2 = 610;
                line.Y1 = 50 + i * 40;
                line.Y2 = line.Y1;

                _board.Children.Add(line);

                line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                line.X1 = 50 + i * 40;
                line.X2 = line.X1;
                line.Y1 = 50;
                line.Y2 = 610;
                _board.Children.Add(line);
            }
        }

        private void PaintDots()
        {
            PaintDot(4, 4);
            PaintDot(8, 8);
            PaintDot(12, 4);
            PaintDot(12, 12);
            PaintDot(4, 12);
        }

        private void PaintDot(int i, int j)
        {
            var dot = new Ellipse();
            dot.Width = 6;
            dot.Height = 6;
            dot.Fill = Brushes.Black;
            Canvas.SetLeft(dot, i * 40 + 10 - 3);
            Canvas.SetTop(dot, j * 40 + 10 - 3);
            _board.Children.Add(dot);
        }
    }
}
