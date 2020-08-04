//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Structure
{
    public class ANDLayer : Layer
    {
        public ICollection<XCellAND> ListOfXCellsAND { get; set; }

        /// <summary>
        /// SortedDictionary <level,<XCellAND>>
        /// </summary>
        private SortedDictionary<double, List<XCellAND>> TemporalListOfXCellsANDGroupedByLevels { get; set; }

        public double Threshold { get; set; }

        public ANDLayer(double threshold=0)
        {
            LayerName = "AND";
            Threshold = threshold;
            ListOfXCellsAND = new List<XCellAND>();
            TemporalListOfXCellsANDGroupedByLevels = new SortedDictionary<double, List<XCellAND>>();
        }

        public string CreateIdOfAllActiveInputs()
        {
            var id = string.Empty;
            var listOfActiveInputs=ListOfInputChannels.Where(inputChannel => inputChannel.Aij > Threshold);
            foreach (var activeChannelInput in listOfActiveInputs)
            {
                id = $"{id}{activeChannelInput.XCellOrigin.Id}&";
            }
            return id.TrimEnd('&');
        }

        public string CreateIdOfAllInactiveInputs()
        {
            var id = string.Empty;
            var listOfInActiveInputs = ListOfInputChannels.Where(inputChannel => inputChannel.Aij < Threshold);
            foreach (var activeChannelInput in listOfInActiveInputs)
            {
                id = $"{id}{activeChannelInput.XCellOrigin.Id}&";
            }
            return id.TrimEnd('&');
        }

        public XCellAND CreateAnXCellANDGivenItsID(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }

            var xCellAND = new XCellAND(id, this);
            xCellAND.AssignLevel();
            ListOfXCellsAND.Add(xCellAND);
            if(xCellAND.ListOfOutputChannels.Count()==0)
            {
                xCellAND.ListOfOutputChannels.Add(new Channel { XCellOrigin = xCellAND });
            }
            LayerUp.ListOfInputChannels.Add(xCellAND.ListOfOutputChannels[0]);
            return xCellAND;
        }

        public XCellAND CreateAnXCellANDGivenItsIDInTemporalList(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }

            var xCellAND = new XCellAND(id, this);
            xCellAND.AssignLevel();
            ListOfXCellsAND.Add(xCellAND);
            if (xCellAND.ListOfOutputChannels.Count() == 0)
            {
                xCellAND.ListOfOutputChannels.Add(new Channel { XCellOrigin = xCellAND });
            }
            //LayerUp.ListOfInputChannels.Add(xCellAND.ListOfOutputChannels[0]);

            TemporalListOfXCellsANDGroupedByLevels.TryGetValue(xCellAND.Li, out var listOfXCellsANDGroupedByLevels);
            if (listOfXCellsANDGroupedByLevels == null)
            {
                var listXCellAND = new List<XCellAND>();
                listXCellAND.Add(xCellAND);
                TemporalListOfXCellsANDGroupedByLevels.Add(xCellAND.Li, listXCellAND);
            }
            else
            {
                listOfXCellsANDGroupedByLevels?.Add(xCellAND);
            }
            return xCellAND;
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsAND.Cast<XCell>().ToList();
        }

        /// <summary>
        /// Se leen los datos de entrada y se construye la estructura necesaria para poder realizar posteriormente SendOutputDataSync
        /// </summary>
        public override void GetInputDataSync() //Diastole
        {
            //CreateAnXCellANDGivenItsIDInTemporalList(CreateIdOfAllActiveInputs());

            var oldLi = double.NaN;
            foreach (var xCellAND in ListOfXCellsAND.OrderBy(xCellAnd => xCellAnd.Li).ToList())
            {
                if(xCellAND.Li != oldLi && oldLi != double.NaN)
                {
                    if(TemporalListOfXCellsANDGroupedByLevels.TryGetValue(oldLi, out var ListXCellAndWithLi))
                    {
                        if(ListXCellAndWithLi!=null)
                        {
                            foreach(var xCellAnd in ListXCellAndWithLi)
                            {
                                xCellAnd.GetInputData();
                            }
                        }
                    }
                    //TODO:
                    //Cuando se termina de procesar las entradas para un nivel, 
                    //se pasa a generar sus salidas para poder seguir procesando las entradas del siguiente nivel
                }
                
                oldLi = xCellAND.Li;
                xCellAND.GetInputData();
            }

            foreach(var levelListXCell in TemporalListOfXCellsANDGroupedByLevels)
            {
                foreach(var xCell in levelListXCell.Value)
                {
                    ListOfXCellsAND.Add(xCell);
                }
            }
            TemporalListOfXCellsANDGroupedByLevels.Clear();
        }

        public override void SendOutputDataSync() //Systole
        {

        }
    }
}
