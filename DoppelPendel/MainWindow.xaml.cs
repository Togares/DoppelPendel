using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace DoppelPendel
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            _viewModel = DataContext as MainWindowViewModel;
            InitializeComponent();
            _viewModel.setup();
            setup();
        }

        private MainWindowViewModel _viewModel;
        private DispatcherTimer _timer;
        private bool _first = true;

        private void setup()
        {
            DrawCanvas.Children.Add(_viewModel.Line1);
            DrawCanvas.Children.Add(_viewModel.Ball1);
            DrawCanvas.Children.Add(_viewModel.Line2);
            DrawCanvas.Children.Add(_viewModel.Ball2);
            Canvas.SetZIndex(_viewModel.Line1, 1);
            Canvas.SetZIndex(_viewModel.Line2, 1);
            Canvas.SetZIndex(_viewModel.Ball1, 1);
            Canvas.SetZIndex(_viewModel.Ball2, 1);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            _timer.Tick += _timer_Tick;
            _viewModel.calculate();
            draw();
            _first = false;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _viewModel.calculate();
            draw();
        }

        private void draw()
        {
            try
            {
                var x1 = _viewModel.Length1 * (Math.Sin(_viewModel.Angle1));
                var y1 = _viewModel.Length1 * (Math.Cos(_viewModel.Angle1));

                _viewModel.Line1.X1 = Math.Floor(Width * 0.5);
                _viewModel.Line1.Y1 = Height * 0.33;
                _viewModel.Line1.X2 = Math.Floor(x1 + _viewModel.Line1.X1);
                _viewModel.Line1.Y2 = Math.Floor(y1 + _viewModel.Line1.Y1);
                Canvas.SetLeft(_viewModel.Ball1, _viewModel.Line1.X2 - _viewModel.BallRadius * 0.5);
                Canvas.SetTop(_viewModel.Ball1, _viewModel.Line1.Y2 - _viewModel.BallRadius * 0.5);

                var x2 = _viewModel.Length2 * (Math.Sin(_viewModel.Angle2));
                var y2 = _viewModel.Length2 * (Math.Cos(_viewModel.Angle2));

                _viewModel.Line2.X1 = Math.Floor(_viewModel.Line1.X2);
                _viewModel.Line2.Y1 = Math.Floor(_viewModel.Line1.Y2);
                _viewModel.Line2.X2 = Math.Floor(x2 + _viewModel.Line1.X2);
                _viewModel.Line2.Y2 = Math.Floor(y2 + _viewModel.Line1.Y2);
                Canvas.SetLeft(_viewModel.Ball2, _viewModel.Line2.X2 - _viewModel.BallRadius * 0.5);
                Canvas.SetTop(_viewModel.Ball2, _viewModel.Line2.Y2 - _viewModel.BallRadius * 0.5);

                if (!_first)
                {
                    _first = false;
                    history();
                }

                _viewModel.Angle1Velocity += _viewModel.Angle1Accelaration;
                _viewModel.Angle2Velocity += _viewModel.Angle2Accelaration;
                _viewModel.Angle1 += _viewModel.Angle1Velocity;
                _viewModel.Angle2 += _viewModel.Angle2Velocity;

                if (_viewModel.UseAirResistance)
                {
                    _viewModel.Angle1Velocity *= 0.9986;
                    _viewModel.Angle2Velocity *= 0.9986;
                }
            }
            catch (ArgumentException)
            {
                Reset_Button_Click(this, new RoutedEventArgs());
                return;
            }
        }

        private void history()
        {
            // Einkommentieren für Verlauf von erster Kugel
            //if (_viewModel.History1.Count <= _viewModel.MaxHistoryCount)
            //{
            //    _viewModel.History1.Add(createHistoryDot(Canvas.GetLeft(_viewModel.Ball1) + _viewModel.BallRadius * 0.5, Canvas.GetTop(_viewModel.Ball1) + _viewModel.BallRadius * 0.5, Brushes.Red));
            //    DrawCanvas.Children.Add(_viewModel.History1.Last());
            //}
            //if (_viewModel.History1.Count >= _viewModel.MaxHistoryCount)
            //{
            //    DrawCanvas.Children.Remove(_viewModel.History1.First());
            //    _viewModel.History1.Remove(_viewModel.History1.First());
            //}

            //Verlauf zweite Kugel
            if (_viewModel.History2.Count <= _viewModel.MaxHistoryCount)
            {
                _viewModel.History2.Add(createHistoryDot(Canvas.GetLeft(_viewModel.Ball2) + _viewModel.BallRadius * 0.5, Canvas.GetTop(_viewModel.Ball2) + _viewModel.BallRadius * 0.5, Brushes.Blue));
                DrawCanvas.Children.Add(_viewModel.History2.Last());
            }
            if (_viewModel.History2.Count >= _viewModel.MaxHistoryCount)
            {
                DrawCanvas.Children.Remove(_viewModel.History2.First());
                _viewModel.History2.Remove(_viewModel.History2.First());
            }
        }

        private Ellipse createHistoryDot(double x, double y, Brush color)
        {
            Ellipse dot = new Ellipse();
            dot.Width = dot.Height = 5;
            dot.Fill = color;
            Canvas.SetLeft(dot, x);
            Canvas.SetTop(dot, y);
            return dot;
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _viewModel.CanValuesChange = false;
            _viewModel.EditValueOpacity = 0.2;
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            DrawCanvas.Children.Remove(_viewModel.Line1);
            DrawCanvas.Children.Remove(_viewModel.Line2);
            DrawCanvas.Children.Remove(_viewModel.Ball1);
            DrawCanvas.Children.Remove(_viewModel.Ball2);
            foreach (var dot in _viewModel.History1)
            {
                DrawCanvas.Children.Remove(dot);
            }
            foreach (var dot in _viewModel.History2)
            {
                DrawCanvas.Children.Remove(dot);
            }
            _viewModel.History1.Clear();
            _viewModel.History2.Clear();
            _viewModel.Angle1 = Angle.DegToRad(_viewModel.Angle1Binding);
            _viewModel.Angle2 = Angle.DegToRad(_viewModel.Angle2Binding);
            _viewModel.Angle1Accelaration = _viewModel.Angle1Velocity = _viewModel.Angle2Accelaration = _viewModel.Angle2Velocity = 0;
            _first = true;
            setup();
            _viewModel.CanValuesChange = true;
            _viewModel.EditValueOpacity = 1;
        }

        private void Angle_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var dot in _viewModel.History1)
            {
                DrawCanvas.Children.Remove(dot);
            }
            foreach (var dot in _viewModel.History2)
            {
                DrawCanvas.Children.Remove(dot);
            }
            _viewModel.calculate();
            draw();
            _first = false;
        }
    }
}
