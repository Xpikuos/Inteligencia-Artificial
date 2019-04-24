//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV2NetStandard.XCells;
using XudonV2NetStandard.Common;

namespace XudonV2NetStandard.Structure
{
    public class FUZZYLayer : Layer
    {
        public Dictionary<string,XCellFuzzyMaster> ListOfXCellFuzzyMasters; //<id,XCellFuzzyMaster>
        private uint _resolution;

        public double SigmaPiN
        {
            get
            {
                var totalXCellFuzzy = 0;
                var sum = 0.0;
                foreach(var xCellFuzzyMaster in ListOfXCellFuzzyMasters)
                {
                    sum += xCellFuzzyMaster.Value.GetSumOfAllXCellFuzzyOutputs();
                    totalXCellFuzzy += xCellFuzzyMaster.Value.ListOfXCellFuzzy.Count;
                }
                return sum / totalXCellFuzzy;
            }
        }

        /// <summary>
        /// Create a FUZZYLayer with an specific resolution for the input values
        /// </summary>
        /// <param name="resolution">From 0 to Number of steps</param>
        public FUZZYLayer(uint layerNumber, uint resolution) : base(layerNumber)
        {
            ListOfXCellFuzzyMasters = new Dictionary<string, XCellFuzzyMaster>();
            _resolution = resolution;
        }

        //public void CreateXCellFuzzyMasters(string idPattern)
        //{
        //    var splittedIdPattern = idPattern.Split('|');
        //    if(splittedIdPattern?.Length>0)
        //    {
        //        foreach(var id in splittedIdPattern)
        //        {
        //            if(!ListOfXCellFuzzyMasters.ContainsKey(id))
        //            {
        //                ListOfXCellFuzzyMasters.Add(id, new XCellFuzzyMaster(id,_resolution));
        //            }
        //        }
        //    }
        //}

        public void CreateXCellFuzzyMasters(List<Pin> listOfPins)
        {
            foreach(var pin in listOfPins)
            {
                if(!ListOfXCellFuzzyMasters.ContainsKey(pin.Id))
                {
                    var xCellFuzzyMaster = new XCellFuzzyMaster(pin, _resolution, this);
                    xCellFuzzyMaster.CreateXCellsFuzzy(pin.Id, _resolution);
                    ListOfXCellFuzzyMasters.Add(pin.Id, xCellFuzzyMaster);
                }
            }
        }

        public void CreateXCellFuzzyMasters(List<Channel> listOfChannels)
        {
            foreach(var channel in listOfChannels)
            {
                if(!ListOfXCellFuzzyMasters.ContainsKey(channel.Pin.Id))
                {
                    var xCellFuzzyMaster = new XCellFuzzyMaster(channel, _resolution,this);
                    xCellFuzzyMaster.CreateXCellsFuzzy(channel.Pin.Id, _resolution);
                    ListOfXCellFuzzyMasters.Add(channel.Pin.Id, xCellFuzzyMaster);
                }
            }
        }

        public void GenerateNewPatternSignalToORLayer(double thresholdX, ORLayer orLayer)
        {
            if(SigmaPiN < thresholdX)
            {
                orLayer.GenerateNewPattern();
            }
        }

        public override void MethodToExecuteAfterReadingAllInputData() //Diastole
        {

        }

        public override void MethodToExecuteAfterSendingAllInputData() //Systole
        {

        }
    }
}
