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
        private Board _boardModel;
        private Rectangle _adorner = new Rectangle() { Width = 40, Height = 40, Stroke = Brushes.AliceBlue, StrokeThickness = 1 };
        private Rectangle _lastMoveAdorner = new Rectangle() { Width = 40, Height = 40, Stroke = Brushes.AliceBlue, StrokeThickness = 1 };
        private BoardStatus _currentPlayer;
        private bool _isEnd;

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
            _isEnd = false;
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
            if (_isEnd)
            {
                return;
            }

            var point = e.GetPosition(_board);
            int i, j;
            GetPoint(point, out i, out j);
            if (i >= 0 && j >= 0 && _boardModel.Data[i, j] == BoardStatus.Empty)
            {
                MoveAdorner(i, j, _adorner);
            }
        }

        private void MoveAdorner(int i, int j, Rectangle adorner)
        {
            _board.Children.Remove(adorner);

            Canvas.SetTop(adorner, 30 + 40 * j);
            Canvas.SetLeft(adorner, 30 + 40 * i);
            adorner.Stroke = _currentPlayer == BoardStatus.Black ? Brushes.Black : Brushes.White;

            _board.Children.Add(adorner);
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
            if (_isEnd)
            {
                return;
            }

            var point = e.GetPosition(_board);
            int i, j;
            GetPoint(point, out i, out j);
            if (i >= 0 && j >= 0 && _boardModel.Data[i, j] == BoardStatus.Empty)
            {
                CreateChessPiece(i, j);
            }

            ComputerGoIfItsTurn();
        }

        private void CreateChessPiece(int i, int j)
        {
            if (_isEnd)
            {
                return;
            }

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

            _boardModel.Set(i, j, _currentPlayer);
            MoveAdorner(i, j, _lastMoveAdorner);
            _currentPlayer = _currentPlayer == BoardStatus.Black ? BoardStatus.White : BoardStatus.Black;
            _adorner.Stroke = _currentPlayer == BoardStatus.Black ? Brushes.Black : Brushes.White;

            var result = _boardModel.WinOrLost(i, j);
            if (result == true)
            {
                MessageBox.Show("player: " + _boardModel.Data[i, j] + " win!");
                _isEnd = true;
            }
            else if (result == false)
            {
                MessageBox.Show("player: " + _boardModel.Data[i, j] + " lost!");
                _isEnd = true;
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Initialize();

            ComputerGoIfItsTurn();
        }

        private void ComputerGoIfItsTurn()
        {
            if (!_isEnd)
            {
                if ((_computerUseBlack.IsChecked == true && _currentPlayer == BoardStatus.Black) ||
                    (_computerUseBlack.IsChecked == false && _currentPlayer == BoardStatus.White))
                {
                    var point = Algorithm.GetBestMove(_currentPlayer, _boardModel);
                    CreateChessPiece(point.i, point.j);
                }
            }
        }
    }
}
