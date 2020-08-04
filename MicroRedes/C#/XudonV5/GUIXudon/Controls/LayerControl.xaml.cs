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
using XudonV4NetFramework.Structure;

namespace GUIXudon.Controls
{
    /// <summary>
    /// Lógica de interacción para Layer.xaml
    /// </summary>
    public partial class LayerControl : UserControl
    {
        public List<LayerControl> ListOfLevelLayersUserControls;

        public LayerControl()
        {
            InitializeComponent();
        }

        public LayerControl(string nameLayer)
        {
            InitializeComponent();
            LayerName.Content = nameLayer;
        }

        public void FillWithGatesContainedInALayer(Layer layer)
        {
            var listOfXCells = layer.GetListOfXCells();
            if (listOfXCells != null)
            {
                foreach (var xCell in listOfXCells)
                {
                    StackPanelInScrollViewerLevel.Children.Add(new Gate(xCell.ListOfInputChannels.Count, 20, 20, xCell.GetType().ToString(), xCell.Id));
                }
            }
        }

        public void AddAGate(Gate gate)
        {
            StackPanelInScrollViewerLevel.Children.Add(gate);
        }

        public void AddLevel(double level)
        {
            if(ListOfLevelLayersUserControls==null)
            {
                ListOfLevelLayersUserControls = new List<LayerControl>();
            }
            var levelLayerUserControl = new LayerControl($"{level}");
            StackPanelInScrollViewerLevel.Children.Add(levelLayerUserControl);
            ListOfLevelLayersUserControls.Add(levelLayerUserControl);
        }
    }
}
