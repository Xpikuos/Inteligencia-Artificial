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
    public class XCellSUM : XCellLogic
    {
        public XCellSUM(Layer layer) :base(layer) { }
        public XCellSUM(string id, Layer layer) : base(id, layer) { }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            var outputValue = double.MaxValue;
            foreach (var inputChannelActive in ListOfInputChannels.Where(inputChannel => inputChannel.IsActive))
            {
                var value = ((inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Pin.Value / Nii);
                if (value < outputValue) { outputValue = value; }
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Pin.Value = outputValue;
                outputChannel.IsActive = outputChannel.Pin.Value >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
            }
        }

        public override void GetInputData() //Diastole
        {

        }

        public override void SendOutputData() //Systole
        {

        }
    }
}
