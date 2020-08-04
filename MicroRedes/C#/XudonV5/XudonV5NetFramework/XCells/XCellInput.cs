//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellInput : XCell
    {
        public XCellInput(Layer layer) : base(layer) { }

        public double MaxInputValue { get; private set; }
        public double MinInputValue { get; private set; }
        public uint R { get; private set; }

        public uint[] CounterOfValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="XCellInput"/> class.
        /// </summary>
        /// <param name="id">id=id1</param>
        /// <param name="layer">The layer.</param>
        public XCellInput(string id, Layer layer) : base(id, layer)
        {
            var minMaxRValues = ((INPUTLayer)Layer).MinAndMaxValuesForInputHeaderID[id];
            MaxInputValue = minMaxRValues.maxValue;
            MinInputValue = minMaxRValues.minValue;
            R = minMaxRValues.R;
            CounterOfValues = new uint[R];
            if (!layer.ListOfInputChannels.Any(channel => channel.XCellOrigin?.Id == id))
            {
                var newInputChannel = new Channel();
                newInputChannel.XCellDestiny = this;
                ListOfInputChannels.Add(newInputChannel);

                var newOutputChannel = new Channel();
                newOutputChannel.XCellOrigin = this;
                ListOfOutputChannels.Add(newOutputChannel);
            }
        }

        public uint GetSumOfAllCounters()
        {
            uint sum = 0;
            foreach(var count in CounterOfValues)
            {
                sum += count;
            }
            return sum;
        }

        public uint GetMappedInputValueAsUInt(double input)
            => Convert.ToUInt32(((input - MinInputValue) * Convert.ToDouble(R) / (MaxInputValue - MinInputValue)));

        public uint UpdateCountersOfMappedInputValue(double input)
        {
            var index = GetMappedInputValueAsUInt(input);

            if (index == R)
            {
                index--;
            }
            CounterOfValues[index]++;
            return index;
        }

        //public uint GetMappedInputValueUpdateCountersAndLimits(double input)
        //{
        //    //Hacer un mapeo del valor de entrada input en un valor dentro del rango [0, resolution-1] que servirá de índice de la matriz _counterOfValues para ir contando las veces que un cierto valor cae dentro del rango
        //    if (input > MaxInputValue) { MaxInputValue = input; }
        //    if (input < MinInputValue) { MinInputValue = input; }
        //    return UpdateCountersOfMappedInputValue(input);
        //}

        public override void AssignInputDependingOnXCellType(double value)
        {
            IN = value;
        }

        public override void GetInputData() //Diastole
        {
            UpdateCountersOfMappedInputValue(ListOfInputChannels[0].Aij);
            AssignInputDependingOnXCellType(ListOfInputChannels[0].Aij); 
        }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            ListOfOutputChannels[0].IsActive = ListOfInputChannels[0].IsActive;
            
            ListOfOutputChannels[0].Aij = GetMappedValue(IN, MinInputValue,MaxInputValue, R);

            ListOfOutputChannels[0].PatternToSendToAnXCell = Id;
        }

        public override void SendOutputData() //Systole
        {
            ActivateOutputChannelsAndGenerateOutputValue();
            OUT = IN;
            if (ListOfOutputChannels[0].XCellDestiny == null)
            {
                Layer.LayerUp.ListOfInputChannels.Add(ListOfOutputChannels[0]);
            }

            //foreach (var inputChannel in ListOfInputChannels)
            //{
            //    inputChannel.PatternToSendToAnXCell = null;
            //    inputChannel.IsActive = false;
            //}
        }

        public override void BuildStructure()
        {
            //if (ListOfOutputChannels[0].XCellDestiny == null)
            //{
            //    Layer.LayerUp.ListOfInputChannels.Add(ListOfOutputChannels[0]);
            //}
        }

        private double GetMappedValue(double input, double minInput, double maxInput, uint R)
            => (input - minInput) * Convert.ToDouble(R) / (maxInput - minInput);
    }
}
