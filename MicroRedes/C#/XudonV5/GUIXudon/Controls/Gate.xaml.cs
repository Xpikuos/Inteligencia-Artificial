//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUIXudon.Controls
{
    /// <summary>
    /// Interaktionslogik für Gate.xaml
    /// </summary>
    public partial class Gate : UserControl
    {
        private const double _lengthOfLine        = 20;
        private const double _strokeThickness     = 2;
        private const double _distanceBetwwenPins = 10;
        private const double _height              = 23;

        private string _id;

        //Borde Azul-> and
        //Borde Negro -> or

        private int _numberOfPins;

        public Gate()
        {
            InitializeComponent();
        }

        public Gate(int numberOfPins, double cornerTopLeftX, double cornerTopLeftY, string type, string id)
        {
            InitializeComponent();
            _id = id;
            _numberOfPins = numberOfPins;
            Height        = _height;
            Margin        = new Thickness(cornerTopLeftX, cornerTopLeftY, 0, 0);

            if(type.ToLower().Contains("and"))
            {
                BorderBrush = Brushes.Blue;
            }
            else if (type.ToLower().Contains("or"))
            {
                BorderBrush = Brushes.Black;
            }

            CanvasGateContainer.PreviewMouseRightButtonDown += CanvasGateContainer_PreviewMouseRightButtonDown;
            Show();
        }

        private void CanvasGateContainer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(_id);
        }

        public void Show()
        {
            Visibility = Visibility.Visible;

            var i = 0;

            Width = (_numberOfPins - 1) * _distanceBetwwenPins + (2 * _distanceBetwwenPins);

            while(i < _numberOfPins)
            {
                var lineIn = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = _strokeThickness,
                    Margin = new Thickness( -Margin.Left, -Margin.Top, 0, 0),//(0,0,0,0),// (-Margin.Top, -Margin.Left, 0, 0), //, Margin.Right, -Margin.Bottom),
                    Y1              = Margin.Top + Height + _lengthOfLine,
                    X1              = Margin.Left + ((i + 1) * _distanceBetwwenPins)- _strokeThickness,
                    Y2              = Margin.Top + Height - _strokeThickness
                };
                lineIn.X2 = lineIn.X1;
                CanvasGateContainer.Children.Add(lineIn);
                i++;
            }

            var lineOut = new Line
            {
                Stroke = Brushes.Black,
                Margin = new Thickness(0,0,0,0),//(0, -_strokeThickness, 0, 0),
                Y1     = -_lengthOfLine,
                X1     = (Width/2)-_strokeThickness,
            };

            lineOut.Y2 = lineOut.Y1 + _lengthOfLine;
            lineOut.X2 = lineOut.X1;
            CanvasGateContainer.Children.Add(lineOut);
        }

        public void Close()
            => Visibility = Visibility.Collapsed;
    }
}
