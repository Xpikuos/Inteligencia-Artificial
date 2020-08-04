//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using GUIXudon.Common;
using GUIXudon.Controls;
using System;
using System.Linq;
using System.Windows;
using XudonV4NetFramework.Structure;
using XudonV4NetFramework.XCells;

//hacer en las layers un método para crear enlaces entre capas automaticamente con sus Xceldas correspondientes
//crear una fase de construcción de estructuras
//la conexión entre XCeldas tiene que hacerse así eXseXs, donde 'e' es un canal de entrada y 's' es un canal de salida y X es una XCelda. De esta forma:
//la conexión entre canales 'se' es virtual: realmente son el mismo canal (el mismo objeto), pero en un canal se 
//definen una variables temporales que permiten implementar las siguientes 3 fases
//Fases:
//a) lectura de datos del canal de entrada (Aij), procesamiento de los mismos y depositar los resultados en una variable temporal (IN)
//b) creación de las nuevas estructuras donde se depositarán los resultados del procesamiento anterior
//d) copiar la variable temporal anterior (IN) en la variable Aij del canal de salida

namespace GUIXudon
{
    public partial class MainWindow : Window
    {
        private Xudon _xudon;

        public MainWindow()
        {
            InitializeComponent();

            var dataManager = new DataManager(@"..\..\..\..\XudonV2_6\GUIXudon\Resources\Data.csv");

            _xudon = new Xudon(dataManager.CloseDBAndDataFile, 
                                  dataManager.ReadLineInDataFile,
                                  dataManager.GetEndOfLine,
                                  dataManager.GetLastLineReadInDataFile,
                                  dataManager.WriteInDB, 
                                  dataManager.AllHeadersIDs, 
                                  dataManager.InputHeadersIDs, 
                                  dataManager.OutputHeadersIDs);

            _xudon.RunXudonThread();

            BuildXudon2DGraph(_xudon);
        }

        private void BuildColumn2DGraph(Column column)
        {
            foreach (var layer in column.ListOfLayers.AsEnumerable().Reverse())
            {
                var layerControl = new LayerControl($"{layer.LayerName}:{layer.LayerNumber}");
                switch(layer.LayerName)
                {
                    case "DP-I":
                        layerControl.LayerName.FontSize = 10;
                        layerControl.Height = 85;
                        layerControl.FillWithGatesContainedInALayer(layer);
                        StackPanelForLayers.Children.Add(layerControl);
                        break;
                    case "DP-II":
                        layerControl.LayerName.FontSize = 10;
                        layerControl.Height = 85;
                        layerControl.FillWithGatesContainedInALayer(layer);
                        StackPanelForLayers.Children.Add(layerControl);
                        break;
                    case "AND":
                        layerControl.Height = 500;
                        layerControl.FillWithGatesContainedInALayer(layer);
                        layerControl.AddLevel((layer.LayerUp.LayerNumber+layer.LayerDown.LayerNumber)/2);
                        StackPanelForLayers.Children.Add(layerControl);
                        break;
                    case "INPUT":
                    case "OR":
                        layerControl.LayerName.FontSize = 15;
                        layerControl.Height = 85;
                        layerControl.FillWithGatesContainedInALayer(layer);
                        StackPanelForLayers.Children.Add(layerControl);
                        break;
                    case "FUZZY":
                        layerControl.LayerName.FontSize = 15;
                        layerControl.Height = 500;
                        var listOfXCellsFuzzyMaster = ((FUZZYLayer)layer).ListOfXCellsFuzzyMaster;
                        if (listOfXCellsFuzzyMaster != null)
                        {
                            foreach (var xCellFuzzyMaster in listOfXCellsFuzzyMaster)
                            {
                                var userControlXCeldaFuzzyMaster = new UserControlXCeldaFuzzyMaster();

                                foreach(var xCellOrExplode in xCellFuzzyMaster.ORExplodeLayer.ListOfXCellsOrExplode)
                                {
                                    var gateOrExplode = new Gate(1, 20, 20, xCellOrExplode.GetType().ToString(), xCellOrExplode.Id);
                                    userControlXCeldaFuzzyMaster.PutAGateInAOrXplodeLevel((layer.LayerUp.LayerNumber + layer.LayerNumber-1) / 2, gateOrExplode);//.LayerControlOrExplode.StackPanelInScrollViewerLevel.Children.Add(gateOrExplode);
                                }

                                double widthXCellsFuzzy = 0;

                                foreach (var xCellFuzzy in xCellFuzzyMaster.ListOfXCellFuzzy)
                                {
                                    var userControlXCeldaFuzzy = new UserControlXCeldaFuzzy(xCellFuzzy.Id);
                                    userControlXCeldaFuzzyMaster.PutAXCellFuzzyInLayerControlForXCellsFuzzy(userControlXCeldaFuzzy);
                                    widthXCellsFuzzy += 53;
                                }

                                (double lowerLimit, double upperLimit) previousRegion = (default(double), default(double));
                                var widthRegionsManagerUC = widthXCellsFuzzy;// userControlXCeldaFuzzyMaster.ActualWidth;// = 100;// userControlXCeldaFuzzyMaster.RegionsManagerUC.ActualWidth;
                                foreach (var region in xCellFuzzyMaster.RegionsManager.ListOfRegions.OrderBy(reg => reg.lowerLimit).ToList())
                                {
                                    userControlXCeldaFuzzyMaster.RegionsManagerUC.AddRegion(widthRegionsManagerUC*(region.lowerLimit - previousRegion.upperLimit) / Convert.ToDouble(xCellFuzzyMaster.RegionsManager.R), false);
                                    userControlXCeldaFuzzyMaster.RegionsManagerUC.AddRegion(widthRegionsManagerUC*(region.upperLimit - region.lowerLimit) / Convert.ToDouble(xCellFuzzyMaster.RegionsManager.R), true);
                                    previousRegion = region;
                                }

                                var widthColumnHistogram = 2* widthRegionsManagerUC/Convert.ToDouble(((XCellInput)(xCellFuzzyMaster.ListOfInputChannels[0].XCellOrigin)).CounterOfValues.Count());
                                var sumOfCountsHistogram = Convert.ToDouble(((XCellInput)(xCellFuzzyMaster.ListOfInputChannels[0].XCellOrigin)).GetSumOfAllCounters());
                                foreach (var count in ((XCellInput)(xCellFuzzyMaster.ListOfInputChannels[0].XCellOrigin)).CounterOfValues)
                                {
                                    userControlXCeldaFuzzyMaster.HistogramUC.AddColumn(count,sumOfCountsHistogram,100.0, widthColumnHistogram);
                                }
                                
                                layerControl.StackPanelInScrollViewerLevel.Children.Add(userControlXCeldaFuzzyMaster);
                            }
                        }
                        StackPanelForLayers.Children.Add(layerControl);
                        break;
                }
            }
        }

        private void BuildXudon2DGraph(Xudon xudon)
        {
            foreach (var column in xudon.ListOfColumns)
            {
                BuildColumn2DGraph(column);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window3d = new Window3D();
            window3d.Show();
            window3d.BuildXudon3DGraph(_xudon);
        }
    }
}
