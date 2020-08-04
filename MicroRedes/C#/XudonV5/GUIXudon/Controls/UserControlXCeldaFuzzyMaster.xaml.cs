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
    /// Interaktionslogik für UserControlXCeldaFuzzyMaster.xaml
    /// </summary>
    public partial class UserControlXCeldaFuzzyMaster : UserControl
    {
        public List<LayerControl> ListOfLevelsForXCellsOrExplode;
        public UserControlRegionsManager RegionsManagerUC;
        public UserControlHistogram HistogramUC;

        public UserControlXCeldaFuzzyMaster()
        {
            InitializeComponent();
            LayerControlOrExplode.LayerName.Content = "OR-EXP";

            LayerControlForXCellsFuzzy.LayerName.Content = "FUZZY";

            RegionsManagerUC = new UserControlRegionsManager();
            LayerControlForRegionsManager.StackPanelInScrollViewerLevel.Children.Add(RegionsManagerUC);
            LayerControlForRegionsManager.LayerName.Content = "RM";

            HistogramUC = new UserControlHistogram();
            LayerControlForHistogram.StackPanelInScrollViewerLevel.Children.Add(HistogramUC);
            LayerControlForHistogram.LayerName.Content = "HIST";

            ListOfLevelsForXCellsOrExplode = new List<LayerControl>();
        }

        public LayerControl AddNewLevelForXCellsOrExplode(double orXplodelevel)
        {
            var newLayerControl = new LayerControl();
            newLayerControl.Height = 75;
            newLayerControl.LayerName.Content = $"{orXplodelevel}";
            LayerControlOrExplode.StackPanelInScrollViewerLevel.Children.Add(newLayerControl);
            ListOfLevelsForXCellsOrExplode.Add(newLayerControl);
            return newLayerControl;
        }

        public void PutAGateInAOrXplodeLevel(double orXplodelevel, Gate gate)
        {
            var levelForXCellsOrExplodeFound = ListOfLevelsForXCellsOrExplode.Find(level => level.LayerName.Content.ToString() == $"{orXplodelevel}");
            if (levelForXCellsOrExplodeFound == null)
            {
                levelForXCellsOrExplodeFound=AddNewLevelForXCellsOrExplode(orXplodelevel);
            }
            levelForXCellsOrExplodeFound.StackPanelInScrollViewerLevel.Children.Add(gate);
        }

        public void PutAXCellFuzzyInLayerControlForXCellsFuzzy(UserControlXCeldaFuzzy userControlXCeldaFuzzy)
        {
            LayerControlForXCellsFuzzy.StackPanelInScrollViewerLevel.Children.Add(userControlXCeldaFuzzy);
        }
    }
}
