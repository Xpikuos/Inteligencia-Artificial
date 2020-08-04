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

namespace GUIXudon.Controls
{
    /// <summary>
    /// Interaktionslogik für UserControlXCeldaFuzzy.xaml
    /// </summary>
    public partial class UserControlXCeldaFuzzy : UserControl
    {
        private Line _horizontalAxis;
        private Line _lineUp;
        private Line _lineDown;
        private Label _minimumLabel;
        private Label _maximumLabel;

        private const double BASE_WIDTH = 100;
        private const double HEIGHT = 100;

        public UserControlXCeldaFuzzy()
        {
            InitializeComponent();
        }

        public UserControlXCeldaFuzzy(double min, double max, string inputId)
        {
            InitializeComponent();

            //GridForCanvasForFuzzyRelation.Margin = new Thickness(10,0,10,0);

            _horizontalAxis = new Line();
            _horizontalAxis.X1 = 0;
            _horizontalAxis.X2 = BASE_WIDTH;
            _horizontalAxis.Y1 = HEIGHT;
            _horizontalAxis.Y2 = HEIGHT;
            _horizontalAxis.Stroke = Brushes.Black;
            _horizontalAxis.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_horizontalAxis);

            _lineUp = new Line();
            _lineUp.X1 = 0;
            _lineUp.X2 = BASE_WIDTH / 2;
            _lineUp.Y1 = HEIGHT;
            _lineUp.Y2 = 0;
            _lineUp.Stroke = Brushes.Black;
            _lineUp.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_lineUp);

            _lineDown = new Line();
            _lineDown.X1 = BASE_WIDTH / 2;
            _lineDown.X2 = BASE_WIDTH;
            _lineDown.Y1 = 0;
            _lineDown.Y2 = HEIGHT;
            _lineDown.Stroke = Brushes.Black;
            _lineDown.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_lineDown);

            _minimumLabel = Text(0, HEIGHT-25, $"{Math.Round(min, 2)}\u2264{inputId}\u2264{Math.Round(max, 2)}", Brushes.Black);
            //_maximumLabel = Text(BASE_WIDTH, HEIGHT, Math.Round(max, 2).ToString(), Brushes.Black);

            //CanvasForFuzzyRelation.Children.Add(_minimumLabel);
            //CanvasForFuzzyRelation.Children.Add(_maximumLabel);

            CanvasForFuzzyRelation.Background = Brushes.Red;
        }

        public UserControlXCeldaFuzzy(string inputId)
        {
            InitializeComponent();

            //GridForCanvasForFuzzyRelation.Margin = new Thickness(10,0,10,0);

            _horizontalAxis = new Line();
            _horizontalAxis.X1 = 0;
            _horizontalAxis.X2 = BASE_WIDTH;
            _horizontalAxis.Y1 = HEIGHT;
            _horizontalAxis.Y2 = HEIGHT;
            _horizontalAxis.Stroke = Brushes.Black;
            _horizontalAxis.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_horizontalAxis);

            _lineUp = new Line();
            _lineUp.X1 = 0;
            _lineUp.X2 = BASE_WIDTH / 2;
            _lineUp.Y1 = HEIGHT;
            _lineUp.Y2 = 0;
            _lineUp.Stroke = Brushes.Black;
            _lineUp.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_lineUp);

            _lineDown = new Line();
            _lineDown.X1 = BASE_WIDTH / 2;
            _lineDown.X2 = BASE_WIDTH;
            _lineDown.Y1 = 0;
            _lineDown.Y2 = HEIGHT;
            _lineDown.Stroke = Brushes.Black;
            _lineDown.StrokeThickness = 1;
            CanvasForFuzzyRelation.Children.Add(_lineDown);

            _minimumLabel = Text(0, HEIGHT - 25, $"{inputId}", Brushes.Black);
            //_maximumLabel = Text(BASE_WIDTH, HEIGHT, Math.Round(max, 2).ToString(), Brushes.Black);

            //CanvasForFuzzyRelation.Children.Add(_minimumLabel);
            //CanvasForFuzzyRelation.Children.Add(_maximumLabel);

            CanvasForFuzzyRelation.Background = Brushes.White;
        }

        private Label Text(double x, double y, string text, Brush color)
        {
            var label = new Label();

            label.Content = text;
            label.Width = BASE_WIDTH;
            label.Foreground = color;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;

            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, y);
            CanvasForFuzzyRelation.Children.Add(label);
            return label;
        }
    }
}
