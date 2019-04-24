//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV2NetStandard.Common;

namespace XudonV2NetStandard.Structure
{
    public class Layer
    {
        //public Thread Thread { get; set; }
        public List<Channel> ListOfInputChannels;
        public List<Channel> ListOfOutputChannels;

        public uint LayerNumber { get; set; }

        public Layer(uint layerNumber)
        {
            LayerNumber = layerNumber;
            ListOfInputChannels = new List<Channel>();
            ListOfOutputChannels = new List<Channel>();
        }

        public void ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(Layer outputLayer)
        {
            foreach(var channel in ListOfInputChannels)
            {
                outputLayer.ListOfInputChannels.Add(new Channel(channel.Pin));
            }
        }

        public void GetInputData() //Diastole
        {
            MethodToExecuteAfterReadingAllInputData();
        }

        public void SendOutputData() //Systole
        {
            MethodToExecuteAfterSendingAllInputData();
        }

        public virtual void ExecuteYourForwardFunctionality()
        { }

        public virtual void MethodToExecuteAfterReadingAllInputData()
        { }

        public virtual void MethodToExecuteAfterSendingAllInputData()
        { }
    }
}
