//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

namespace XudonV2NetStandard.Structure
{
    public class MicroRed
    {
        /// <summary>
        /// Number of learned patterns
        /// </summary>
        /// <value>
        /// The n.
        /// </value>
        public uint N { get; set; }

        public INPUTLayer INPUTLayer { get; set; }

        public FUZZYLayer FUZZYLayer { get; set; }

        public DIFUSSORLayer DIFUSSORLayer { get; set; }

        public ANDLayer ANDLayer { get; set; }

        public ORLayer ORLayer { get; set; }

        public MicroRed()
        {
            INPUTLayer = new INPUTLayer(0,@"E:\XudonV2\XudonV2NetStandard\Resources\Pins.xml");

            FUZZYLayer = new FUZZYLayer(1,255);
            INPUTLayer.ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(FUZZYLayer);
            DIFUSSORLayer = new DIFUSSORLayer(2);
            FUZZYLayer.ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(DIFUSSORLayer);
            ANDLayer = new ANDLayer(3);
            DIFUSSORLayer.ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(ANDLayer);
            ORLayer = new ORLayer(4);
            ANDLayer.ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(ORLayer);
        }

        public void GetInputData() //Diastole
        {
            INPUTLayer.GetInputData();
        }

        public void SendOutputData() //Systole
        {
            ORLayer.SendOutputData();
        }
    }
}
