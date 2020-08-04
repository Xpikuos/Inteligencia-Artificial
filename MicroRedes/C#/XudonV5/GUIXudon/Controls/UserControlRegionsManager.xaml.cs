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
    /// Interaktionslogik für UserControlRegionsManager.xaml
    /// </summary>
    public partial class UserControlRegionsManager : UserControl
    {
        public UserControlRegionsManager()
        {
            InitializeComponent();
        }

        public void AddRegion(double value, bool freeOrBusy, bool showBorderOfRegion=true) //free=false, busy=true
        {
            if(value<0)
            {
                value = 0;
            }

            var rectangle = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 20,
                Width = value
            };

            if(showBorderOfRegion)
            {
                rectangle.StrokeThickness = 1;
                rectangle.Stroke = Brushes.White;
            }

            if(freeOrBusy) //Busy
            {
                rectangle.Fill = Brushes.OrangeRed;
            }
            else //Free
            {
                rectangle.Fill = Brushes.LightGreen;
            }

            StackPannelForRectangles.Children.Add(rectangle);
        }

        public void AddLastRegion(double value)
        {
            var rectangle = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 20,
                Width = value
            };

            rectangle.Fill = Brushes.LightGreen;
            

            StackPannelForRectangles.Children.Add(rectangle);
        }
    }
}
