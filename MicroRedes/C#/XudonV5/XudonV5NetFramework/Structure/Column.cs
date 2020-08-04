//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XudonV4NetFramework.Structure
{
    public class Column
    {
        public List<Layer> ListOfLayers { get; set; }

        public Column()
        {
            ListOfLayers = new List<Layer>();
        }

        public void AddLayerAndConnectItWithThePreviousOne(Layer layer)
        {
            if (ListOfLayers.Count > 0)
            {
                ListOfLayers[ListOfLayers.Count - 1].ConnectThisLayerWithOutputLayer(layer);
            }
            ListOfLayers.Add(layer);
        }

        //public void GetInputDataThread() //Diastole
        //{
        //    Thread = new Thread(() =>
        //    {
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.GetInputDataThread();
        //        }
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.Thread?.Join();
        //        }
        //    });
        //    Thread.Start();
        //}

        //public void SendOutputDataThread() //Systole
        //{
        //    Thread = new Thread(() =>
        //    {
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.SendOutputDataThread();
        //        }
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.Thread?.Join();
        //        }
        //    });
        //    Thread.Start();
        //}

        //public void BuildStructureThread() //Systole
        //{
        //    Thread = new Thread(() =>
        //    {
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.BuildStructureThread();
        //        }
        //        foreach (var layer in ListOfLayers)
        //        {
        //            layer.Thread?.Join();
        //        }
        //    });
        //    Thread.Start();
        //}

        //public void GetInputDataSync() //Diastole
        //{
        //    foreach (var layer in ListOfLayers)
        //    {
        //        layer.GetInputDataSync();
        //    }
        //}

        //public void BuildStructureSync() //Diastole
        //{
        //    foreach (var layer in ListOfLayers)
        //    {
        //        layer.BuildStructureSync();
        //    }
        //}

        //public void SendOutputDataSync() //Systole
        //{
        //    foreach (var layer in ListOfLayers)
        //    {
        //        layer.SendOutputDataSync();
        //    }
        //}
    }
}
