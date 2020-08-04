//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellFUp : XCell
    {
        private uint[] _counterOfValues;
        private uint _resolution;
        private double _maxInput;
        private double _minInput;

        private double _m_rampUp;
        private double _n_rampUp;

        private double _m_rampDown;
        private double _n_rampDown;

        private uint _uLeft;
        private uint _uCenter;
        private uint _uRight;

        public double OutputFuzzyValue { get; set; }

        public XCellFUp(Layer layer) :base(layer) { }
        public XCellFUp(string id, Layer layer) : base(id, layer) { }

        public XCellFUp(uint resolution, Layer layer) : base(layer)
        {
            _resolution      = resolution;
            _counterOfValues = new uint[resolution];
            _maxInput        = double.MinValue;
            _minInput        = double.MaxValue;
        }

        public uint GetMappedInputValue(double input) => (uint)((input - _minInput) * _resolution / (_maxInput - _minInput));

        public uint UpdateCountersOfMappedInputValue(double input)
        {
            var index = GetMappedInputValue(input);
            _counterOfValues[index]++;
            return index;
        }

        public uint GetMappedInputValueUpdateCountersAndLimits(double input)
        {
            //Hacer un mapeo del valor de entrada input en un valor dentro del rango [0, resolution-1] que servirá de índice de la matriz _counterOfValues para ir contando las veces que un cierto valor cae dentro del rango
            if (input > _maxInput) { _maxInput = input; }
            if (input < _minInput) { _minInput = input; }
            return UpdateCountersOfMappedInputValue(input);
        }

        public void BuildFuzzyRelation(double center, double left, double right)
        {
            _uCenter = GetMappedInputValue(center);
            _uLeft   = GetMappedInputValue(left);
            _uRight  = GetMappedInputValue(right);

            _m_rampUp = 1 / (_uCenter - _uLeft);
            _n_rampUp = -_uLeft * _m_rampUp;

            _m_rampDown = 1 / (_uCenter - _uRight);
            _n_rampDown = -_uRight * _m_rampUp;
        }

        public double GetFuzzyValue(double input)
        {
            var uInput = GetMappedInputValue(input);

            if(uInput>= _uRight || uInput<= _uLeft)
            {
                OutputFuzzyValue = 0;
            }
            else if(uInput> _uLeft && uInput<=_uCenter)
            {
                OutputFuzzyValue = _m_rampUp * (uInput - _uLeft);
            }
            else if(uInput < _uRight && uInput >= _uCenter)
            {
                OutputFuzzyValue = _m_rampDown * (uInput - _uRight);
            }
            else
            {
                OutputFuzzyValue = 0;
            }

            return OutputFuzzyValue;
        }

        public override void GetInputData() //Diastole
        {
        }

        public override void SendOutputData() //Systole
        {

            foreach (var inputChannel in ListOfInputChannels)
            {
                inputChannel.PatternToSendToAnXCell = null;
                inputChannel.IsActive = false;
            }
        }
    }
}
