//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV2NetStandard.Common;
using XudonV2NetStandard.Structure;

namespace XudonV2NetStandard.XCells
{
    public class XCellFuzzyMaster : XCell
    {
        public bool NewPattern;
        public Dictionary<string, XCellFuzzy> ListOfXCellFuzzy; //<id,XCellFuzzy>

        private uint _resolution;
        private double _maxInput;
        private double _minInput;

        private uint _lastMappedInputValue;

        public XCellFuzzyMaster(uint resolution,Layer layer):base(layer)
        {
            ListOfXCellFuzzy = new Dictionary<string, XCellFuzzy>();
            _resolution      = resolution;
        }

        public XCellFuzzyMaster(string id, uint resolution, Layer layer) : base(id, layer)
        {
            ListOfXCellFuzzy = new Dictionary<string, XCellFuzzy>();
            _resolution      = resolution;
        }

        public XCellFuzzyMaster(Pin pin, uint resolution, Layer layer) : base(pin.Id, layer)
        {
            ListOfXCellFuzzy = new Dictionary<string, XCellFuzzy>();
            _resolution      = resolution;

            AssignPinToInputChannelsOfTheXCell(pin);
        }

        public XCellFuzzyMaster(Channel channel, uint resolution, Layer layer) : base(channel.Pin.Id, layer)
        {
            ListOfXCellFuzzy = new Dictionary<string, XCellFuzzy>();
            _resolution      = resolution;

            AssignChannelToInputChannelsOfTheXCell(channel);
        }

        /// <summary>
        /// Create XCellsFuzzy with an id=id-Mapped value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">It can ONLY be >=0 </param>
        public void CreateXCellsFuzzy(string id, double value)
        {
            _lastMappedInputValue = GetMappedInputValue(value);
            var idXCellFuzzy      = $"{id}={_lastMappedInputValue}";
            if(!ListOfXCellFuzzy.ContainsKey(idXCellFuzzy))
            {
                var xCellFuzzy = new XCellFuzzy(idXCellFuzzy, Layer);
                ListOfXCellFuzzy.Add(idXCellFuzzy,xCellFuzzy);
            }
        }

        public double GetSumOfAllXCellFuzzyOutputs()
        {
            var sum = 0.0;
            foreach(var xCellFuzzy in ListOfXCellFuzzy)
            {
                sum += xCellFuzzy.Value.OutputFuzzyValue;
            }
            return sum;
        }

        public override void GetInputData() //Diastole
        {

        }

        public override void SendOutputData() //Systole
        {

        }

        private uint GetMappedInputValue(double input)
        {
            if(input > _maxInput) { _maxInput = input; }
            if(input < _minInput) { _minInput = input; }
            return (uint)((input - _minInput) * _resolution / (_maxInput - _minInput));
        }
    }
}