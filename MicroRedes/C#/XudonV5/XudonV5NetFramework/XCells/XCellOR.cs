//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellOR : XCellLogic
    {
        public XCellOR(Layer layer) : base(layer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCellOR"/> class.
        /// </summary>
        /// <param name="idSequence">id=id1|id2|...|idn</param>
        /// <param name="layer">The layer.</param>
        public XCellOR(string idSequence, Layer layer) : base(idSequence, layer)
        {
            var ids = idSequence.Split('|');
            foreach(var id in ids)
            {
                var newInputChannel = new Channel();
                newInputChannel.XCellDestiny = this;
                var channelForInputFoundInLayerAsInputChannel=layer.ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == id);
                if (channelForInputFoundInLayerAsInputChannel != null)
                {
                    newInputChannel.XCellOrigin = channelForInputFoundInLayerAsInputChannel.XCellOrigin;
                }
                ListOfInputChannels.Add(newInputChannel);
            }

            var newOutputChannel = new Channel();
            newOutputChannel.XCellOrigin = this;
            ListOfOutputChannels.Add(newOutputChannel);
        }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            var outputValue = double.MaxValue;
            foreach (var inputChannelActive in ListOfInputChannels.Where(inputChannel => inputChannel.IsActive))
            {
                var value = (inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Aij / Nii;
                if (value < outputValue) { outputValue = value; }
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Aij = outputValue;
                outputChannel.IsActive = outputChannel.Aij >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
            }
        }

        public override void AssignInputDependingOnXCellType(double value)
        {
            if(value > IN) //Criterio como OR fuzzy: se toma el mayor
            {
                IN = value;
            }
        }

        public override void GetInputData() //Diastole
        {
            AuxPattern = string.Empty;
            AuxIsActive = false;
            double maxValue = double.MinValue;
            foreach (var inputChannel in ListOfInputChannels)
            {
                if (inputChannel.IsActive)
                {
                    AuxIsActive = true;
                    if (inputChannel.Aij > maxValue)
                    {
                        maxValue = inputChannel.Aij;
                    }
                    if (!string.IsNullOrEmpty(inputChannel.PatternToSendToAnXCell))
                    {
                        AuxPattern = $"{AuxPattern}{inputChannel.PatternToSendToAnXCell}|";
                    }
                }
            }
            AuxPattern = AuxPattern.TrimEnd('|');
            IN = maxValue;
        }

        public override void SendOutputData() //Systole
        {
            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.PatternToSendToAnXCell = AuxPattern;
                outputChannel.IsActive = AuxIsActive;
                outputChannel.Aij = IN;
            }

            foreach (var inputChannel in ListOfInputChannels)
            {
                inputChannel.PatternToSendToAnXCell = null;
                inputChannel.IsActive = false;
                inputChannel.Aij = double.NaN;
            }
        }
    }
}
