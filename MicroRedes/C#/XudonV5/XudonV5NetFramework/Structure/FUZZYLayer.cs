//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV4NetFramework.XCells;
using XudonV4NetFramework.Common;
using System.Linq;
using System;

namespace XudonV4NetFramework.Structure
{
    public class FUZZYLayer : Layer
    {
        public ICollection<XCellFuzzyMaster> ListOfXCellsFuzzyMaster;

        public double SigmaPiN
        {
            get
            {
                var totalXCellFuzzy = 0;
                var sum = 0.0;
                var listOfXCellFuzzyMastersActive = ListOfXCellsFuzzyMaster.Where(xCellFuzzyMaster => xCellFuzzyMaster.IsActive);
                foreach(var xCellFuzzyMaster in listOfXCellFuzzyMastersActive)
                {
                    sum += xCellFuzzyMaster.OUT;// GetSumOfAllXCellFuzzyOutputs();
                    totalXCellFuzzy += xCellFuzzyMaster.ListOfXCellFuzzy.Count;
                }
                return sum / totalXCellFuzzy;
            }
        }

        /// <summary>
        /// Create a FUZZYLayer with an specific resolution for the input values
        /// </summary>
        public FUZZYLayer()
        {
            LayerName = "FUZZY";
            ListOfXCellsFuzzyMaster = new List<XCellFuzzyMaster>();
        }

        //public void CreateXCellFuzzyMaster(string id)
        //{
        //    if (!ListOfXCellFuzzyMasters.Any(xCellFuzzyMaster => xCellFuzzyMaster.Id == id))
        //    {
        //        var xCellFuzzyMaster = new XCellFuzzyMaster(id, this);
        //        xCellFuzzyMaster.AssignLevel();
        //        ListOfXCellFuzzyMasters.Add(xCellFuzzyMaster);
        //    }
        //}

        //public void CreateXCellFuzzyMasters(List<Channel> listOfChannels)
        //{
        //    foreach (var channel in listOfChannels)
        //    {
        //        var id = channel.XCellOrigin.Id;
        //        if (!ListOfXCellFuzzyMasters.Any(xCellFuzzyMaster => xCellFuzzyMaster.Id == id))
        //        {
        //            var xCellFuzzyMaster = new XCellFuzzyMaster(channel, this);
        //            xCellFuzzyMaster.AssignLevel();
        //            //xCellFuzzyMaster.CreateXCellsFuzzy(id, HyperParameters.R);
        //            ListOfXCellFuzzyMasters.Add(xCellFuzzyMaster);
        //        }
        //    }
        //}

        public void GenerateNewPatternSignalToORLayer(double thresholdX, ORLayer orLayer)
        {
            if(SigmaPiN < thresholdX)
            {
                orLayer.GenerateNewPattern();
            }
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsFuzzyMaster.Cast<XCell>().ToList();
        }

        public override void GetInputDataSync() //Diastole
        {
            var listOfInputChannelsActive = ListOfInputChannels.Where(inputChannel => inputChannel.IsActive == true).ToList();
            if (listOfInputChannelsActive != null)
            {
                foreach (var inputChannelActive in listOfInputChannelsActive)
                {
                    if (inputChannelActive.XCellDestiny == null)
                    {
                        var xCellFuzzyMaster = new XCellFuzzyMaster(inputChannelActive.XCellOrigin.Id, this, ((XCellInput)(inputChannelActive.XCellOrigin)).R);
                        xCellFuzzyMaster.AssignLevel();
                        xCellFuzzyMaster.ListOfInputChannels.Add(inputChannelActive);
                        inputChannelActive.XCellDestiny = xCellFuzzyMaster;
                        ListOfXCellsFuzzyMaster.Add(xCellFuzzyMaster);
                        xCellFuzzyMaster.ORExplodeLayer.ConnectThisLayerWithOutputLayer(this);
                    }
                    inputChannelActive.XCellDestiny.GetInputData();
                }
            }
        }

        public override void BuildStructureSync()
        {
            //var listOfInputChannelsActive = ListOfInputChannels.Where(inputChannel => inputChannel.IsActive == true).ToList();
            //if (listOfInputChannelsActive != null)
            //{
            //    foreach (var inputChannelActive in listOfInputChannelsActive)
            //    {
            //        if (inputChannelActive.XCellDestiny == null)
            //        {
            //            var xCellFuzzyMaster = new XCellFuzzyMaster(inputChannelActive.XCellOrigin.Id, this, ((XCellInput)(inputChannelActive.XCellOrigin)).R);
            //            xCellFuzzyMaster.AssignLevel();
            //            xCellFuzzyMaster.ListOfInputChannels.Add(inputChannelActive);
            //            inputChannelActive.XCellDestiny = xCellFuzzyMaster;
            //            ListOfXCellsFuzzyMaster.Add(xCellFuzzyMaster);
            //            xCellFuzzyMaster.ORExplodeLayer.ConnectThisLayerWithOutputLayer(this);
            //        }
            //    }
            //}
        }

        public override void SendOutputDataSync() //Systole
        {
            foreach (var xCellFuzzyMaster in ListOfXCellsFuzzyMaster)
            {
                xCellFuzzyMaster.SendOutputData();
            }
        }
    }
}
