//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Structure
{
    public class Layer: IDisposable
    {
        /// <summary>
        /// Output layer-Next layer
        /// </summary>
        public Layer LayerUp { get; set; }

        /// <summary>
        /// Input layer-Previous layer
        /// </summary>
        public Layer LayerDown { get; set; }

        public BlockingCollection<Channel> ListOfInputChannels;
        public BlockingCollection<Channel> ListOfOutputChannels;

        protected ICollection<XCell> ListOfXCells;

        public double LayerNumber { get; set; }

        public string LayerName { get; set; }

        public Layer()
        {
            ListOfInputChannels = new BlockingCollection<Channel>();
            ListOfOutputChannels = new BlockingCollection<Channel>();
        }

        public virtual void ConnectThisLayerWithOutputLayer(Layer outputLayer)
        {
            outputLayer.LayerNumber = LayerNumber + 1;
            LayerUp = outputLayer;
            outputLayer.LayerDown=this;
        }

        public void PrepareTaskToReadInput()
        {
            Xudon.ListOfTasksToReadInputs.Add(new Task(GetInputDataSync));
        }

        public void PrepareTaskToBuildStructures()
        {
            Xudon.ListOfTasksToBuildStructures.Add(new Task(BuildStructureSync));
        }

        public void PrepareTaskToGenerateOutputs()
        {
            Xudon.ListOfTasksToGenerateOutputs.Add(new Task(SendOutputDataSync));
        }

        //public void StartTaskToReadInputs() //Diastole
        //{
        //    _taskToReadInputs.Start();
        //}

        //public void SendOutputDataThread() //Systole
        //{
        //    Thread = new Thread(new ThreadStart(SendOutputDataSync));
        //    Thread.Start();
        //}

        //public void BuildStructureThread()
        //{
        //    Thread = new Thread(new ThreadStart(BuildStructureSync));
        //    Thread.Start();
        //}

        public virtual ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCells;
        }

        public virtual void GetInputDataSync()
        { }

        public virtual void SendOutputDataSync()
        { }

        public virtual void BuildStructureSync()
        { }

        public virtual void Dispose() => throw new NotImplementedException();
    }
}
