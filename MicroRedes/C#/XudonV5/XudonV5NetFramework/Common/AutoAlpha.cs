//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;

namespace XudonV4NetFramework.Common
{
    public class AutoAlpha
    {
        public double Alpha { get; set; }
        private uint[] _counterOfValues;
        private uint _maxCounter1;
        private uint _maxCounter2;
        private uint _indexOfMaxCounter1;
        private uint _indexOfMaxCounter2;
        private double _maxInput;
        private double _minInput;
        private uint _maxInputMapped;
        private uint _minInputMapped;

        public AutoAlpha()
        {
            _counterOfValues = new uint[HyperParameters.R];
        }

        public void CalculateAlpha() => Alpha = (_indexOfMaxCounter1 * _maxCounter1 + _indexOfMaxCounter2 * _maxCounter2) / (_maxCounter1 + _maxCounter2);

        private uint GetMappedInputValue(double input) => (uint)((input - _minInput) * HyperParameters.R / (_maxInput - _minInput));

        private uint UpdateCountersOfMappedInputValue(double input)
        {
            var index = GetMappedInputValue(input);
            if(index>= _counterOfValues.Length)
            {
                Array.Resize(ref _counterOfValues, Convert.ToInt32(index+1));
            }
            _counterOfValues[index]++;
            if(_counterOfValues[index] > _maxCounter1)
            {
                _maxCounter2        = _maxCounter1;
                _indexOfMaxCounter2 = _indexOfMaxCounter1;
                _maxCounter1        = _counterOfValues[index];
                _indexOfMaxCounter1 = index;
            }
            return index;
        }

        public uint GetMappedInputValueUpdateCountersAndLimits(double input)
        {
            //Hacer un mapeo del valor de entrada input en un valor dentro del rango [0, resolution-1] que servirá de índice de la matriz _counterOfValues para ir contando las veces que un cierto valor cae dentro del rango
            var mappedValue = UpdateCountersOfMappedInputValue(input);
            if(input > _maxInput) { _maxInput = input; _maxInputMapped = mappedValue; }
            if(input < _minInput) { _minInput = input; _minInputMapped = mappedValue; }
            return mappedValue;
        }
    }
}
