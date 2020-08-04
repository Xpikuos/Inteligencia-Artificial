//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellFuzzy : XCell
    {

        private double _maxInput;
        private double _minInput;

        private double _m_rampUp;
        private double _n_rampUp;

        private double _m_rampDown;
        private double _n_rampDown;

        private double _R;

        public double _uLeft;
        public double _uCenter;
        public double _uRight;

        //public double OutputFuzzyValue { get; set; }

        public XCellFuzzy(string id, Layer layer, uint R) : base(id, layer)
        {
            //_counterOfValues = new uint[R];
            
            _R= Convert.ToDouble(R);
            _maxInput = _R;// double.MinValue;
            _minInput = 0;// double.MaxValue;

            var newOutputChannel = new Channel();
            newOutputChannel.XCellOrigin = this;
            ListOfOutputChannels.Add(newOutputChannel);

            layer.ListOfOutputChannels.Add(newOutputChannel);
            layer.LayerUp.ListOfInputChannels.Add(newOutputChannel);

            var lowCenterUp = id.Split('<');//lowerLimit<id<uppderLimit
            BuildFuzzyRelation(Convert.ToDouble(lowCenterUp[0]),Convert.ToDouble(lowCenterUp[2]));
        }

        public double GetMappedInputValueAsDouble(double input, double R)
            => (input - _minInput) * R / (_maxInput - _minInput);

        public void BuildFuzzyRelation(double left, double right)
        {
            //_uCenter = GetMappedInputValue(center);
            //_uLeft   = GetMappedInputValue(left);
            //_uRight  = GetMappedInputValue(right);

            _uCenter = (left+right)/2;
            _uLeft   = left;
            _uRight  = right;

            _m_rampUp = 1 / (_uCenter - _uLeft);
            _n_rampUp = -_uLeft * _m_rampUp;

            _m_rampDown = 1 / (_uCenter - _uRight);
            _n_rampDown = -_uRight * _m_rampUp;
        }

        public double GetFuzzyValue(double input)
        {
            //GetMappedInputValueUpdateCountersAndLimits(input);
            //UpdateCountersOfMappedInputValue(input);

            var kk = this;

            var uInput = GetMappedInputValueAsDouble(input,_R);// GetMappedInputValueAsUInt(input);

            if(uInput>= _uRight || uInput<= _uLeft)
            {
                return 0;
            }
            else if(uInput> _uLeft && uInput<=_uCenter)
            {
                return _m_rampUp * (uInput - _uLeft);
            }
            else if(uInput < _uRight && uInput >= _uCenter)
            {
                return _m_rampDown * (uInput - _uRight);
            }
            else
            {
                return 0;
            }
        }

        private bool IsWholeNumber(double x)
        {
            return Math.Abs(x % 1) < double.Epsilon;
        }

        public override void GetInputData() //Diastole
        {
            IN = ListOfInputChannels[0].Aij;
            //SendOutputData();
        }

        public override void SendOutputData() //Systole
        {
            OUT = GetFuzzyValue(IN);
            ListOfOutputChannels[0].Aij = OUT;

            var kk = this;

            if(OUT==0 || OUT==double.NaN)
            {
                ListOfOutputChannels[0].PatternToSendToAnXCell = null;
                ListOfOutputChannels[0].IsActive = false;
            }
            else
            {
                ListOfOutputChannels[0].PatternToSendToAnXCell = Id;
                ListOfOutputChannels[0].IsActive = true;
            }

            //ListOfInputChannels[0].PatternToSendToAnXCell = null;
            //ListOfInputChannels[0].IsActive = false;
            //ListOfInputChannels[0].Aij = double.NaN;

            //foreach (var inputChannel in ListOfInputChannels)
            //{
            //    inputChannel.PatternToSendToAnXCell = null;
            //    inputChannel.IsActive = false;
            //}
        }
    }
}
