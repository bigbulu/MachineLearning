﻿using System;
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
        private Board _boardModel;
        private Rectangle _adorner = new Rectangle() { Width = 40, Height = 40, Stroke = Brushes.AliceBlue, StrokeThickness = 1 };
        private BoardStatus _currentPlayer;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            _board.Children.Clear();
            PaintGridLine();
            PaintDots();
            _currentPlayer = BoardStatus.Black;
            _boardModel = new Board();
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

        private void _board_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(_board);
            int i, j;
            GetPoint(point, out i, out j);
            if (i >= 0 && j >= 0 && _boardModel.Data[i,j] == BoardStatus.Empty)
            {
                MoveAdorner(i, j);
            }
        }

        private void MoveAdorner(int i, int j)
        {
            _board.Children.Remove(_adorner);

            Canvas.SetTop(_adorner, 30 + 40 * j);
            Canvas.SetLeft(_adorner, 30 + 40 * i);
            _adorner.Stroke = _currentPlayer == BoardStatus.Black ? Brushes.Black : Brushes.White;

            _board.Children.Add(_adorner);
        }

        private void GetPoint(Point point, out int i, out int j)
        {
            i = (int)((point.X - 30) / 40);
            j = (int)((point.Y - 30) / 40);
            if (i < 0 || i >= 15) i = -1;
            if (j < 0 || j >= 15) j = -1;
        }

        private void _board_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(_board);
            int i, j;
            GetPoint(point, out i, out j);
            if (i >= 0 && j >= 0 && _boardModel.Data[i, j] == BoardStatus.Empty)
            {
                CreateChessPiece(i, j);
            }
        }

        private void CreateChessPiece(int i, int j)
        {
            var chessPiece = new Ellipse()
            {
                Width = 30,
                Height = 30,
                Fill = _currentPlayer == BoardStatus.Black ? Brushes.Black : Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Canvas.SetLeft(chessPiece, 35 + 40 * i);
            Canvas.SetTop(chessPiece, 35 + 40 * j);
            _board.Children.Add(chessPiece);

            _currentPlayer = _currentPlayer == BoardStatus.Black ? BoardStatus.White : BoardStatus.Black;
            _adorner.Stroke = _currentPlayer == BoardStatus.Black ? Brushes.Black : Brushes.White;

            var result = _boardModel.WinOrLost(i, j);
            if (result == true)
            {
                MessageBox.Show("player: " + _boardModel.Data[i, j] + " has win!");
            }
            else if (result == false)
            {

                MessageBox.Show("player: " + _boardModel.Data[i, j] + " has lost!");
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
        }
    }
}
