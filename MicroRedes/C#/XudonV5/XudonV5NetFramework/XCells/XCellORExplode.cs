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
    public class XCellORExplode : XCellOR
    {
        public XCellORExplode(Layer layer) : base(layer) { }
        public XCellORExplode(string id, Layer layer) : base(id, layer)
        {
            var newInputChannel = new Channel();
            newInputChannel.XCellDestiny = this;
            var channelForInputFoundInLayerAsInputChannel = layer.ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == id);
            if(channelForInputFoundInLayerAsInputChannel!=null)
            {
                newInputChannel.XCellOrigin = channelForInputFoundInLayerAsInputChannel.XCellOrigin;
            }
            ListOfInputChannels.Add(newInputChannel);

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

        public override void AssignLevel()
        {
            var Lj = double.MaxValue;
            var Lh = 0.0;

            foreach(var inputChannel in ListOfInputChannels)
            {
                var LhTemp = 0.0;
                if(inputChannel.XCellOrigin == null)
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

            Li = (Lj + Lh) / 2;
        }

        public override void GetInputData() //Diastole
        {
            AuxPattern = string.Empty;
            AuxIsActive = false;
            double maxValue = double.MinValue;
            foreach (var inputChannel in ListOfInputChannels)
            {
                if(inputChannel.IsActive)
                {
                    AuxIsActive = true;
                    if (inputChannel.Aij> maxValue)
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
            foreach(var outputChannel in ListOfOutputChannels)
            {
                outputChannel.PatternToSendToAnXCell = AuxPattern;
                outputChannel.IsActive = AuxIsActive;
                outputChannel.Aij = IN;
            }

            foreach (var inputChannel in ListOfInputChannels)
            {
                inputChannel.PatternToSendToAnXCell = null;
                inputChannel.IsActive = false;
                inputChannel.Aij = 0;
            }
        }

        /// <summary>
        /// ID=a<x<w~a<x<b,c<x<d,...,z<x<w
        /// </summary>
        /// <param name="ID">The identifier.</param>
        private void Explode(string ID)
        {
            var IDoldIDnew = ID.Split('~');
            if(IDoldIDnew.Count()==2)
            {
                var inputChannelOfTheOriginalXCellOrToExplodeToReconnect=ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == IDoldIDnew[0]);
                if(inputChannelOfTheOriginalXCellOrToExplodeToReconnect != null)
                {
                    var newXCellOrExplode = new XCellORExplode(IDoldIDnew[1], Layer);
                    inputChannelOfTheOriginalXCellOrToExplodeToReconnect.XCellOrigin = newXCellOrExplode;
                    newXCellOrExplode.ListOfOutputChannels.Add(inputChannelOfTheOriginalXCellOrToExplodeToReconnect);

                    var newRegions = IDoldIDnew[1].Split(',');
                    foreach(var region in newRegions)
                    {
                        var channelConnectedToAnXceldaFuzzy=Layer.ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == region);
                        if(channelConnectedToAnXceldaFuzzy!=null)
                        {
                            var newInputChannelForNewXCellOrExplode = new Channel();
                            newInputChannelForNewXCellOrExplode.XCellDestiny = newXCellOrExplode;
                            newInputChannelForNewXCellOrExplode.XCellOrigin = channelConnectedToAnXceldaFuzzy.XCellOrigin;
                            newXCellOrExplode.ListOfInputChannels.Add(newInputChannelForNewXCellOrExplode);
                        }
                    }

                    newXCellOrExplode.AssignLevel();
                }
            }
        }
    }
}
