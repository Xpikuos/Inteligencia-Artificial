//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Collections.Generic;
using XudonV2NetStandard.Common;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Linq;
using System.Threading;

namespace XudonV2NetStandard.Structure
{
    public class INPUTLayer
    {
        public List<Pin> ListOfInputPins;
        public Thread Thread { get; set; }

        public uint LayerNumber { get; set; }

        public INPUTLayer(uint layerNumber, string jsonFileOfInputPinDefinition)
        {
            LayerNumber = layerNumber;

            var xml = XDocument.Load(jsonFileOfInputPinDefinition);
            var schemas = new XmlSchemaSet();

            schemas.Add("urn:pin-schema", jsonFileOfInputPinDefinition.Replace(".xml", ".xsd"));

            var msg = "";
            xml.Validate(schemas, (o, e) => {
                msg += e.Message + Environment.NewLine;
            });

            if(msg?.Length == 0)
            {
                var pins = xml.Root.Descendants("Pin").Elements().Where(element => element.Name == "id");
                foreach(var pin in pins)
                {
                    ListOfInputPins.Add(new Pin(pin.Value));
                }
                return;
            }

            Console.WriteLine(msg?.Length == 0 ? "Document is valid" : "Document invalid: " + msg);

            //// Query the data and write out a subset of contacts
            //var query = from c in xml.Root.Descendants("Pin")
            //            where (int)c.Attribute("id") < 4
            //            select c.Element("firstName").Value + " " +
            //                   c.Element("lastName").Value;

        }

        public void ConnectThisLayerWithAnotherLayerCreatingChannelsWithPins(Layer outputLayer)
        {
            foreach(var inputPin in ListOfInputPins)
            {
                outputLayer.ListOfInputChannels.Add(new Channel(inputPin));
            }
        }

        public void GetInputData() //Diastole
        {
            //Thread = new Thread(nombreDelmetodoQueHaraAlgo);
            //Thread.Start();
            //Thread.Join();

            //Read values and put them into Pins

            foreach(var inputPin in ListOfInputPins)
            {
                inputPin.Value = new Random().Next(0, 100);
            }
        }

        public void SendOutputData() //Systole
        {

        }
    }
}
