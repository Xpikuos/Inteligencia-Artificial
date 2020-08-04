//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

//http://www.sqlitetutorial.net/download-install-sqlite/

using System;
using System.Collections.Generic;
using XudonV4NetFramework.Common;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Structure
{
    public class INPUTLayer:Layer
    {
        public ICollection<XCellInput> ListOfXCellsInput { get; set; }

        public string LineWithValues { get; set; }
        /// <summary>
        /// Dictionary with <headerID, [minValue,maxValue,R]>
        /// </summary>
        public Dictionary<string, (double minValue,double maxValue, uint R)> MinAndMaxValuesForInputHeaderID { get; set; }

        private Action _closeDB;
        private Func<string> _readLineInDataFile;
        private Action<string> _writeInDB;
        //private StreamReader _readerDataFile;

        /// <summary>
        /// Each element of the list is a group of values separated with comas (just like a line of the Data.csv)
        /// </summary>
        private List<string> _listOfValuesToRetry;

        public INPUTLayer(Action closeDB, Func<string> readLineInDataFile, Action<string> writeInDB, string dataFile, string jsonFileOfInputPinDefinition, List<string> allHeadersIDs, List<string> inputHeadersIDs)
        {
            LayerName = "INPUT";

            _readLineInDataFile = readLineInDataFile;
            _closeDB = closeDB;
            _writeInDB = writeInDB;

            _listOfValuesToRetry = new List<string>();

            ListOfXCellsInput = new List<XCellInput>();

            LayerNumber = 0;
            //GetHeadersIDs(dataFile, out var allHeadersIDs, out inputHeadersIDs, out var outputHeadersIDs);
            
            MinAndMaxValuesForInputHeaderID = GetMinInputMaxInputForHeaderIDsAndR(allHeadersIDs, jsonFileOfInputPinDefinition);
            CreateXCellsWithIDsAndInputAndOutputChannels(inputHeadersIDs);

            //try
            //{
            //    //_readerDataFile = new StreamReader(dataFile);
            //    //var headerLine = _readerDataFile.ReadLine(); //jump the header line
            //    //_headerLineWithoutSymbols = headerLine.Replace("_i", string.Empty).Replace("_o",string.Empty);//Trim('>', '<');
            //}
            //catch(IOException)
            //{
            //    _closeDB();
            //    //_readerDataFile.Close();
            //}
        }

        public void CreateXCellsWithIDsAndInputAndOutputChannels(List<string> inputHeadersIDs)
        {
            foreach (var inputHeaderID in inputHeadersIDs)
            {
                var xCellInput = new XCellInput(inputHeaderID, this);
                xCellInput.AssignLevel();
                ListOfXCellsInput.Add(xCellInput);
                ListOfInputChannels.Add(xCellInput.ListOfInputChannels[0]);
                ListOfOutputChannels.Add(xCellInput.ListOfOutputChannels[0]);
            }
        }

        private Dictionary<string, (double minValue, double maxValue, uint R)> GetMinInputMaxInputForHeaderIDsAndR(List<string> headerIDs, string jsonFileOfInputPinDefinition)
        {
            Dictionary<string, (double minValue, double maxValue, uint R)> minAndMaxRValuesForHeaderID = null;

            var xml = XDocument.Load(jsonFileOfInputPinDefinition);
            var schemas = new XmlSchemaSet();
            schemas.Add("urn:pin-schema", jsonFileOfInputPinDefinition.Replace(".xml", ".xsd"));

            var msg = "";
            xml.Validate(schemas, (o, e) => { msg += e.Message + Environment.NewLine; });

            if (msg?.Length == 0)
            {
                minAndMaxRValuesForHeaderID = new Dictionary<string, (double minValue, double maxValue, uint R)>();
                foreach (var headerID in headerIDs)
                {
                    var headerIDTrimmed = headerID.Replace("_i",string.Empty);
                    var pin = xml.Root.Descendants("Pin").Elements().Where(element => element.Name == "id" && element.Value == headerIDTrimmed).FirstOrDefault();
                    if (pin != null)
                    {
                        var maxValue = Convert.ToDouble(pin.Parent.Descendants("value").Elements().Where(element => element.Name == "maxValue").ElementAt(0).Value);
                        var minValue = Convert.ToDouble(pin.Parent.Descendants("value").Elements().Where(element => element.Name == "minValue").ElementAt(0).Value);
                        var R = Convert.ToUInt32(pin.Parent.Descendants("value").Elements().Where(element => element.Name == "R").ElementAt(0).Value);
                        minAndMaxRValuesForHeaderID.Add(headerIDTrimmed, ( minValue, maxValue, R ));
                    }
                }
            }

            return minAndMaxRValuesForHeaderID;
        }

        public override void ConnectThisLayerWithOutputLayer(Layer outputLayer)
        {
            outputLayer.LayerNumber = LayerNumber + 1;
            LayerUp = outputLayer;
        }

        public override void GetInputDataSync() //Diastole
        {
            if(_listOfValuesToRetry.Count>0)
            {
                foreach(var lineWithValues in _listOfValuesToRetry)
                {
                    FillListOfInputChannelsWithValues(lineWithValues);
                }
                _listOfValuesToRetry.Clear();
            }
            else
            {
                LineWithValues = _readLineInDataFile();
                if (LineWithValues != null)
                {
                    _writeInDB(LineWithValues);
                    FillListOfInputChannelsWithValues(LineWithValues);
                }
            }
        }

        private void FillListOfInputChannelsWithValues(string lineWithValues)
        {
            var values = lineWithValues.Split(HyperParameters.Separator);
            var index = 0;
            foreach (var xCellInput in ListOfXCellsInput)
            {
                xCellInput.ListOfInputChannels[0].Aij = Convert.ToDouble(values[index]);
                xCellInput.ListOfInputChannels[0].IsActive = true;
                xCellInput.ListOfInputChannels[0].PatternToSendToAnXCell = xCellInput.Id;
                xCellInput.GetInputData();
                index++;
            }
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsInput.Cast<XCell>().ToList();
        }

        public override void SendOutputDataSync() //Systole
        {
            foreach(var xCellInput in ListOfXCellsInput)
            {
                xCellInput.SendOutputData();
            }
        }

        public override void Dispose()
        {
            //_readerDataFile.Close();
            _closeDB();
        }
    }
}
