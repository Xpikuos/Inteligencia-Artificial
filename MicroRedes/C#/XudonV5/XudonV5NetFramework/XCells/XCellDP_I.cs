//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Common.Structure;
using XudonV4NetFramework.Structure;

namespace XudonV4NetFramework.XCells
{
    public class XCellDP_I : XCell
    {
        public XCellDP_I(Layer layer) : base(layer) { }
        public XCellDP_I(string id, Layer layer) : base(id, layer)
        {
            //if (!layer.ListOfInputChannels.Any(channel => channel.XCellOrigin?.Id == id))
            //{
            //    var newInputChannel = new Channel();
            //    newInputChannel.XCellDestiny = this;
            //    ListOfInputChannels.Add(newInputChannel);

                var newOutputChannel = new Channel();
                newOutputChannel.XCellOrigin = this;
                ListOfOutputChannels.Add(newOutputChannel);
            //}
        }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            ListOfOutputChannels[0].IsActive = ListOfInputChannels[0].IsActive;

            ListOfOutputChannels[0].Aij = ListOfInputChannels[0].Aij;

            ListOfOutputChannels[0].PatternToSendToAnXCell = ListOfInputChannels[0].PatternToSendToAnXCell;// ((DIFUSSOR_I_Layer)Layer).PatternToSendInOutputChannels;
        }

        public override void AssignInputDependingOnXCellType(double value)
        {
            IN = value;
        }

        public override void GetInputData() //Diastole
        {
            AssignInputDependingOnXCellType(ListOfInputChannels[0].Aij);
        }

        public override void SendOutputData() //Systole
        {
            ActivateOutputChannelsAndGenerateOutputValue();
            OUT = IN;

            //if (ListOfOutputChannels[0].XCellDestiny == null)
            //{
            //    Layer.LayerUp.ListOfInputChannels.Add(ListOfOutputChannels[0]);
            //}

            //foreach (var inputChannel in ListOfInputChannels)
            //{
            //    inputChannel.PatternToSendToAnXCell = null;
            //    inputChannel.IsActive = false;
            //    inputChannel.Aij = double.NaN;
            //}
        }
    }
}
