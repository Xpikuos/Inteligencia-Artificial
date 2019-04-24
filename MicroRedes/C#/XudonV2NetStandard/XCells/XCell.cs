//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Collections.Generic;
using System.Linq;
using XudonV2NetStandard.Common;
using XudonV2NetStandard.Common.Structure;
using XudonV2NetStandard.Structure;

namespace XudonV2NetStandard.XCells
{
    public class XCell
    {
        public static uint Resolution { get; set; }
        public double Li { get; set; } //nivel al cual pertenece la XCelda dentro de la estructura arbórea que se va formando
        public string Id { get; set; }

        public double IN
        {
            get
            {
                return _in;
            }
            set
            {
                AssignInputDependingOnXCellType(value);
                _autoAlpha.GetMappedInputValueUpdateCountersAndLimits(_in);
                _autoAlpha.CalculateAlpha();
            }
        }

        public double OUT { get; set; } //punto en donde la XCelda deposita el resultado de sus cálculos internos. Sería el equivalente al axón. Los canales de salida tomarán como valor de entrada el valor de esta variable
        public bool IsActive { get; set; }

        //Cada vez que se da una relación ij, se incrementan los contadores Nii y Njj 
        //(es decir, los contadores Nii de las dos XCeldas que aparecen en lo extremos del canal ij)
        public ulong Nii { get; set; }

        public IList<Channel> ListOfOutputChannels { get; set; }

        public IList<Channel> ListOfInputChannels { get; set; }

        protected Atomizer Atomizer { get; set; }

        protected Router Router { get; set; }

        public Layer Layer { get; set; } //capa a la que pertenece la XCelda. Necesaria para que una XCelda añada dentro de ella a otra XCelda cuando decida crear una nueva

        private double _in;

        private AutoAlpha _autoAlpha;

        public XCell(Layer layer)
        {
            _autoAlpha = new AutoAlpha(Resolution);

            Layer = layer;
            Nii   = 1;

            ListOfOutputChannels = new List<Channel>();
            ListOfInputChannels  = new List<Channel>();

            Atomizer = new Atomizer();
            Router   = new Router();
        }

        public XCell(string id, Layer layer)
        {
            _autoAlpha = new AutoAlpha(Resolution);

            Layer = layer;
            Id    = id;
            Nii   = 1;

            ListOfOutputChannels = new List<Channel>();
            ListOfInputChannels  = new List<Channel>();

            Atomizer = new Atomizer();
            Router   = new Router();
        }

        public virtual void ActivateOutputChannelsAndGenerateOutputValue()
        {
            var outputValue = 0.0;
            foreach (var inputChannelActive in ListOfInputChannels.Where(inputChannel => inputChannel.IsActive))
            {
                outputValue += (inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Pin.Value / Nii;
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Pin.Value = outputValue;
                outputChannel.IsActive  = outputChannel.Pin.Value >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
            }
        }

        public void AssignChannelsToInputChannelsOfTheXCell(List<Channel> listOfChannels)
        {
            foreach(var channel in listOfChannels)
            {
                channel.XCellDestiny = this;
                ListOfInputChannels.Add(channel);
            }
        }

        public void AssignChannelToInputChannelsOfTheXCell(Channel channel)
        {
            channel.XCellDestiny = this;
            ListOfInputChannels.Add(channel);
        }

        public void AssignPinsToInputChannelsOfTheXCell(List<Pin> listOfPins)
        {
            foreach(var pin in listOfPins)
            {
                var channel = new Channel(pin);
                channel.XCellDestiny = this;
                ListOfInputChannels.Add(channel);
            }
        }

        public void AssignPinToInputChannelsOfTheXCell(Pin pin)
        {
            var channel = new Channel(pin);
            channel.XCellDestiny = this;
            ListOfInputChannels.Add(channel);
        }

        public void ResetIN()
        {
            _in = 0;
        }

        public void ResetOUT()
        {
            OUT = 0;
        }

        public virtual void ExecuteYourForwardFunctionality()
        {
            if(IN>=_autoAlpha.Alpha)
            {
                OUT = Sigmoid(IN);
                IsActive = true;
            }
            else
            {
                OUT = 0;
                IsActive = false;
            }
            foreach(var outputChannel in ListOfOutputChannels)
            {
                outputChannel.ExecuteYourForwardFunctionality();
            }
        }

        public virtual void ExecuteYourBackwardFunctionality()
        {
            foreach(var inputChannel in ListOfInputChannels)
            {
                inputChannel.ExecuteYourBackwardFunctionality();
                _in = 0;
                OUT = 0;
                IsActive = false;
            }
        }

        public virtual void GetInputData() //Diastole
        {
            foreach(var inputChannel in ListOfInputChannels)
            {
                inputChannel.ExecuteYourForwardFunctionality();
            }
        }

        public virtual void SendOutputData() //Systole
        {
            ExecuteYourForwardFunctionality();
        }

        public virtual void AssignInputDependingOnXCellType(double value)
        {
            _in += value;
        }

        public virtual void AssignLevel()
        {
            Li = Layer.LayerNumber;
        }

        private double Sigmoid(double value)
        {
            return 1 / (1 + Math.Exp(-(value - _autoAlpha.Alpha))); //desplazamos la sigmoide para que quede centrada en el valor umbral alpha
        }

        private void DynamicOversight() //Olvido dinámico
        { }

        private void Apoptosis()
        { }
    }
}
