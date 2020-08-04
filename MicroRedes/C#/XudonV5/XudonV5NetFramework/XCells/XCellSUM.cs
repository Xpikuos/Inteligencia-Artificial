//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Linq;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellSUM : XCellLogic
    {
        public XCellSUM(Layer layer) :base(layer) { }
        public XCellSUM(string id, Layer layer) : base(id, layer) { }

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
