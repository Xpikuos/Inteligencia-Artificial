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
    /// Interaktionslogik für UserControlHistogram.xaml
    /// </summary>
    public partial class UserControlHistogram : UserControl
    {
        public UserControlHistogram()
        {
            InitializeComponent();
        }

        public void AddColumn(double value, double total, double scaleFactor, double width, bool showBorderOfRegion = true) //free=false, busy=true
        {
            if (value < 0)
            {
                value = 0;
            }

            var frecuency = value / total;

            var rectangle = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = scaleFactor * frecuency,
                Width = width,
                Fill = Brushes.LightSeaGreen,
                ToolTip = $"{value}:{total}={frecuency}"
            };

            if (showBorderOfRegion)
            {
                rectangle.StrokeThickness = 1;
                rectangle.Stroke = Brushes.White;
            }

            StackPannelForRectangles.Children.Add(rectangle);
        }
    }
}
