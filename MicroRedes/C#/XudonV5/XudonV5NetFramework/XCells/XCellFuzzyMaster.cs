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
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellFuzzyMaster : XCell
    {
        public bool NewPattern { get; set; }
        public ICollection<XCellFuzzy> ListOfXCellFuzzy { get; set; }
        public RegionsManager RegionsManager { get; set; }
        public ORExplodeLayer ORExplodeLayer { get; set; }

        private double _maxInput;
        private double _minInput;

        //private uint _lastMappedInputValue;

        public XCellFuzzyMaster(string id, Layer layer, uint R) : base(id, layer)
        {
            RegionsManager = new RegionsManager(R);
            ListOfXCellFuzzy = new List<XCellFuzzy>();
            ORExplodeLayer = new ORExplodeLayer();
        }

        //public double GetSumOfAllXCellFuzzyOutputs()
        //{
        //    var sum = 0.0;
        //    foreach(var xCellFuzzy in ListOfXCellFuzzy)
        //    {
        //        sum += xCellFuzzy.OutputFuzzyValue;
        //    }
        //    return sum;
        //}

        //public override void GetInputData() //Diastole
        //{
        //    if(this.ListOfInputChannels.Count>0)
        //    {
        //        IN = ListOfInputChannels[0].Aij;

        //        var lowerLimit = IN - RegionsManager.r;
        //        var upperLimit = IN + RegionsManager.r;
        //        if (RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit))//RegionsManager.CheckIfRegionIsAvailable(lowerLimit, upperLimit))
        //        {
        //            var xCellFuzzyNew = new XCellFuzzy($"{lowerLimit}<{Id}<{upperLimit}", Layer, RegionsManager.R);
        //            xCellFuzzyNew.ListOfInputChannels.Add(ListOfInputChannels[0]);
        //            //RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit);
        //            ListOfXCellFuzzy.Add(xCellFuzzyNew);
        //            //xCellFuzzyNew.GetInputData();
        //            //Recalculate_r();
        //        }

        //        foreach (var xCellFuzzy in ListOfXCellFuzzy)
        //        {
        //            xCellFuzzy.GetInputData();
        //            xCellFuzzy.SendOutputData();
        //        }
        //    }
        //}

        //public override void SendOutputData() //Systole
        //{
        //    //var lowerLimit = IN - RegionsManager.r;
        //    //var upperLimit = IN + RegionsManager.r;
        //    //if (RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit))//RegionsManager.CheckIfRegionIsAvailable(lowerLimit, upperLimit))
        //    //{
        //    //    var xCellFuzzyNew = new XCellFuzzy($"{lowerLimit}<{Id}<{upperLimit}", Layer, RegionsManager.R);
        //    //    xCellFuzzyNew.ListOfInputChannels.Add(ListOfInputChannels[0]);
        //    //    //RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit);
        //    //    ListOfXCellFuzzy.Add(xCellFuzzyNew);
        //    //    //xCellFuzzyNew.GetInputData();
        //    //    //Recalculate_r();
        //    //}

        //    //foreach (var xCellFuzzy in ListOfXCellFuzzy)
        //    //{
        //    //    xCellFuzzy.GetInputData();
        //    //    xCellFuzzy.SendOutputData();
        //    //}

        //    XCellFuzzy xCellFuzzyWithGreaterOutput = null;
        //    double greaterOutput = 0;
        //    foreach (var xCellFuzzy in ListOfXCellFuzzy.Where(xCellFuzz => xCellFuzz.ListOfOutputChannels[0].IsActive && xCellFuzz.ListOfOutputChannels[0].Aij > 0))
        //    {
        //        if (!ORExplodeLayer.ListOfXCellsOrExplode.Any(xCellORExplode => xCellORExplode.Id == xCellFuzzy.Id))
        //        {
        //            var xCellOrExplode = new XCellORExplode(xCellFuzzy.Id, this.Layer);
        //            ORExplodeLayer.ListOfXCellsOrExplode.Add(xCellOrExplode);
        //            ORExplodeLayer.ListOfInputChannels.Add(xCellFuzzy.ListOfOutputChannels[0]);
        //        }
        //        if (xCellFuzzy.ListOfOutputChannels[0].Aij > greaterOutput)
        //        {
        //            greaterOutput = xCellFuzzy.ListOfOutputChannels[0].Aij;
        //            xCellFuzzyWithGreaterOutput = xCellFuzzy;
        //        }
        //    }

        //    if (xCellFuzzyWithGreaterOutput != null)
        //    {
        //        var outputChannelFound = ListOfOutputChannels.FirstOrDefault(outputChannel => outputChannel.XCellOrigin.Id == xCellFuzzyWithGreaterOutput.Id);
        //        if (outputChannelFound == null)
        //        {
        //            ListOfOutputChannels.Add(xCellFuzzyWithGreaterOutput.ListOfOutputChannels[0]);
        //        }
        //    }
        //}

        public override void GetInputData() //Diastole
        {
            if (this.ListOfInputChannels.Count > 0)
            {
                IN = ListOfInputChannels[0].Aij;

                var lowerLimit = IN - RegionsManager.r;
                var upperLimit = IN + RegionsManager.r;
                if (RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit))//RegionsManager.CheckIfRegionIsAvailable(lowerLimit, upperLimit))
                {
                    var xCellFuzzyNew = new XCellFuzzy($"{lowerLimit}<{Id}<{upperLimit}", Layer, RegionsManager.R);
                    xCellFuzzyNew.ListOfInputChannels.Add(ListOfInputChannels[0]);
                    //RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit);
                    ListOfXCellFuzzy.Add(xCellFuzzyNew);
                    //xCellFuzzyNew.GetInputData();
                    //Recalculate_r();
                }

                foreach (var xCellFuzzy in ListOfXCellFuzzy)
                {
                    xCellFuzzy.GetInputData();
                    xCellFuzzy.SendOutputData();
                }
            }
        }

        public override void SendOutputData() //Systole
        {
            //var lowerLimit = IN - RegionsManager.r;
            //var upperLimit = IN + RegionsManager.r;
            //if (RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit))//RegionsManager.CheckIfRegionIsAvailable(lowerLimit, upperLimit))
            //{
            //    var xCellFuzzyNew = new XCellFuzzy($"{lowerLimit}<{Id}<{upperLimit}", Layer, RegionsManager.R);
            //    xCellFuzzyNew.ListOfInputChannels.Add(ListOfInputChannels[0]);
            //    //RegionsManager.TryToAddANewRegion(lowerLimit, upperLimit);
            //    ListOfXCellFuzzy.Add(xCellFuzzyNew);
            //    //xCellFuzzyNew.GetInputData();
            //    //Recalculate_r();
            //}

            //foreach (var xCellFuzzy in ListOfXCellFuzzy)
            //{
            //    xCellFuzzy.GetInputData();
            //    xCellFuzzy.SendOutputData();
            //}

            XCellFuzzy xCellFuzzyWithGreaterOutput = null;
            double greaterOutput = 0;
            foreach (var xCellFuzzy in ListOfXCellFuzzy.Where(xCellFuzz => xCellFuzz.ListOfOutputChannels[0].IsActive && xCellFuzz.ListOfOutputChannels[0].Aij > 0))
            {
                if (!ORExplodeLayer.ListOfXCellsOrExplode.Any(xCellORExplode => xCellORExplode.Id == xCellFuzzy.Id))
                {
                    var xCellOrExplode = new XCellORExplode(xCellFuzzy.Id, this.Layer);
                    ORExplodeLayer.ListOfXCellsOrExplode.Add(xCellOrExplode);
                    ORExplodeLayer.ListOfInputChannels.Add(xCellFuzzy.ListOfOutputChannels[0]);
                }
                if (xCellFuzzy.ListOfOutputChannels[0].Aij > greaterOutput)
                {
                    greaterOutput = xCellFuzzy.ListOfOutputChannels[0].Aij;
                    xCellFuzzyWithGreaterOutput = xCellFuzzy;
                }
            }

            if (xCellFuzzyWithGreaterOutput != null)
            {
                var outputChannelFound = ListOfOutputChannels.FirstOrDefault(outputChannel => outputChannel.XCellOrigin.Id == xCellFuzzyWithGreaterOutput.Id);
                if (outputChannelFound == null)
                {
                    ListOfOutputChannels.Add(xCellFuzzyWithGreaterOutput.ListOfOutputChannels[0]);
                }
            }
        }

        private uint GetMappedInputValue(double input, double R)
        {
            if(input > _maxInput) { _maxInput = input; }
            if(input < _minInput) { _minInput = input; }
            return (uint)((input - _minInput) * R / (_maxInput - _minInput));
        }

        private double DeFuzzyfier()
        {
            var value = 0;

            foreach(var outputChannel in ListOfOutputChannels)
            {
                var backPropagatedValue = outputChannel.Aij;
                //TODO
            }

            return value;
        }
    }
}