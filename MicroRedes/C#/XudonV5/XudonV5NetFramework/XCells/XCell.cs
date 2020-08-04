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
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Common.Structure;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCell
    {
        /// <summary>
        /// Nivel al cual pertenece la XCelda dentro de la estructura arbórea que se va formando
        /// </summary>
        public double Li { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// Get or set the cummulative value of values of all input channels
        /// </summary>
        public double IN { get; set; }

        /// <summary>
        /// punto en donde la XCelda deposita el resultado de sus cálculos internos. Sería el equivalente al axón. Los canales de salida tomarán como valor de entrada el valor de esta variable
        /// </summary>
        public double OUT { get; set; }
        public bool IsActive { get; set; }

        //Cada vez que se da una relación ij, se incrementan los contadores Nii y Njj 
        //(es decir, los contadores Nii de las dos XCeldas que aparecen en lo extremos del canal ij)
        public ulong Nii { get; set; }

        public IList<Channel> ListOfOutputChannels { get; set; }

        public IList<Channel> ListOfInputChannels { get; set; }

        protected Atomizer Atomizer { get; set; }

        protected Router Router { get; set; }

        public Layer Layer { get; set; } //capa a la que pertenece la XCelda. Necesaria para que una XCelda añada dentro de dicha capa a otra XCelda cuando decida crear una nueva

        public AutoAlpha _autoAlpha;

        public XCell(Layer layer)
        {
            _autoAlpha = new AutoAlpha();

            Layer = layer;
            Nii   = 1;

            ListOfOutputChannels = new List<Channel>();
            ListOfInputChannels  = new List<Channel>();

            Atomizer = new Atomizer();
            Router   = new Router();
        }

        public XCell(string idSequence, Layer layer)
        {
            _autoAlpha = new AutoAlpha();

            Layer = layer;
            Id    = idSequence;
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
                outputValue += (inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Aij / Nii;
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Aij = outputValue;
                outputChannel.IsActive  = outputChannel.Aij >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
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

        public void ResetIN() => IN = 0;

        public void ResetOUT() => OUT = 0;


        public virtual void ExecuteYourBackwardFunctionality()
        {
            foreach(var inputChannel in ListOfInputChannels)
            {
                inputChannel.ExecuteYourBackwardFunctionality();
                IN = 0;
                OUT = 0;
                IsActive = false;
            }
        }

        public virtual void GetInputData() //Diastole
        {
            foreach(var inputChannel in ListOfInputChannels)
            {
                var value = inputChannel.Aij;
                AssignInputDependingOnXCellType(value);
                _autoAlpha.GetMappedInputValueUpdateCountersAndLimits(value);
                _autoAlpha.CalculateAlpha();
            }
        }

        public virtual void BuildStructure() { }

        public virtual void SendOutputData() //Systole
        {
            if (IN >= _autoAlpha.Alpha)
            {
                if (HyperParameters.UseNonLinearFunctionForAij)
                {
                    OUT = Sigmoid(IN);
                }
                else
                {
                    OUT = IN;
                }
                IsActive = true;
            }
            else
            {
                OUT = 0;
                IsActive = false;
            }
            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.ExecuteYourForwardFunctionality();
            }

            foreach (var inputChannel in ListOfInputChannels)
            {
                inputChannel.PatternToSendToAnXCell = null;
                inputChannel.IsActive = false;
            }
        }

        public virtual void AssignInputDependingOnXCellType(double value) => IN += value;

        public virtual void AssignLevel()
            => Li = Layer.LayerNumber;

        private double Sigmoid(double value)
            => 1 / (1 + Math.Exp(-(value - _autoAlpha.Alpha))); //desplazamos la sigmoide para que quede centrada en el valor umbral alpha

        private int Sign(int? value)
        {
            if (value == null)
            {
                return 0;
            }

            if (value >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private int Sign(double value)
        {
            if (value >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private void DynamicOversight() //Olvido dinámico
        { }

        private void Apoptosis()
        { }
    }
}
