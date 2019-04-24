//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Linq;
using XudonV2NetStandard.Structure;

namespace XudonV2NetStandard.XCells
{
    public class XCellAND : XCellLogic
    {
        public XCellAND(Layer layer) :base(layer) { }
        public XCellAND(string id, Layer layer) : base(id, layer) { }

        public void Autogenesis()
        {

        }

        /// <summary>
        /// Atomices this instance.
        /// Atomización.
        /// </summary>
        public void Atomice()
        {

        }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            var outputValue = double.MinValue;
            foreach (var inputChannelActive in ListOfInputChannels.Where(inputChannel => inputChannel.IsActive))
            {
                var value = ((inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Pin.Value / Nii);
                if (value > outputValue) { outputValue = value; }
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Pin.Value = outputValue;
                outputChannel.IsActive = outputChannel.Pin.Value >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
            }
        }

        public override void AssignInputDependingOnXCellType(double value)
        {
            if(value < IN) //Criterio como AND fuzzy: se toma el menor
            {
                IN = value;
            }
        }

        public override void AssignLevel()
        {
            var Lj = double.MaxValue;
            var Lh = 0.0;

            foreach(var inputChannel in ListOfInputChannels)
            {
                var LhTemp = 0.0;
                if(inputChannel.XCellOrigin==null)
                {
                    LhTemp = Layer.LayerNumber - 1;
                }
                else
                {
                    LhTemp = inputChannel.XCellOrigin.Li;
                }
                if(LhTemp > Lh)
                {
                    Lh = LhTemp;
                }
            }

            foreach(var outputChannel in ListOfOutputChannels)
            {
                var LjTemp = 0.0;
                if(outputChannel.XCellDestiny == null)
                {
                    LjTemp = Layer.LayerNumber + 1;
                }
                else
                {
                    LjTemp = outputChannel.XCellDestiny.Li;
                }
                if(LjTemp < Lj)
                {
                    Lj = LjTemp;
                }
            }

            Li = (Lj+Lh)/2;
        }

        public override void GetInputData() //Diastole
        {

        }

        public override void SendOutputData() //Systole
        {

        }
    }
}
