using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using XudonV4NetFramework.Common;

namespace XudonV4NetFramework.Structure
{
    public class Xudon
    {
        public static List<Task> ListOfTasksToReadInputs;
        public static List<Task> ListOfTasksToBuildStructures;
        public static List<Task> ListOfTasksToGenerateOutputs;

        public INPUTLayer InputLayer { get; set; }
        /// <summary>
        /// Number of learned patterns
        /// </summary>
        public static uint N { get; set; }

        public List<Column> ListOfColumns { get; set; } = new List<Column>();

        private Func<bool> _getEndOfLine;

        public Xudon(Action CloseDB, Func<string> ReadLineInDataFile, Func<bool> getEndOfLine, Func<string> getLastLineReadInDataFile, Action<string> WriteInDB, List<string> allHeadersIDs, List<string> inputHeadersIDs, List<string> outputHeadersIDs)
        {
            ListOfTasksToReadInputs = new List<Task>();
            ListOfTasksToBuildStructures = new List<Task>();
            ListOfTasksToGenerateOutputs = new List<Task>();

            _getEndOfLine = getEndOfLine;

            HyperParameters.R     = 2;
            HyperParameters.pi    = 0;
            HyperParameters.r     = 1;
            HyperParameters.alpha = 0;
            HyperParameters.nii   = 50;

            //Un Xudon puede tener varias columnas. Todas se ejecutan en paralelo
            var column = new Column();
            InputLayer = new INPUTLayer(CloseDB, ReadLineInDataFile, WriteInDB, @"..\..\..\..\XudonV2_6\GUIXudon\Resources\Data.csv", @"..\..\..\..\XudonV2_5\GUIXudon\Resources\Pins.xml", allHeadersIDs, inputHeadersIDs);
            column.AddLayerAndConnectItWithThePreviousOne(InputLayer);
            column.AddLayerAndConnectItWithThePreviousOne(new FUZZYLayer());
            column.AddLayerAndConnectItWithThePreviousOne(new DIFUSSOR_I_Layer());
            column.AddLayerAndConnectItWithThePreviousOne(new ANDLayer());
            column.AddLayerAndConnectItWithThePreviousOne(new DIFUSSOR_II_Layer());
            column.AddLayerAndConnectItWithThePreviousOne(new ORLayer(getLastLineReadInDataFile, outputHeadersIDs));

            ListOfColumns.Add(column);
        }

        public void RunXudonThread(bool stepByStep=false)
        {
            try
            {
                do
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    ManageTasksToReadInputs();
                    ManageTasksToBuildStructures();
                    ManageTasksToGenerateOutputs();
                    watch.Stop();
                    Debug.WriteLine($"Tiempo en procesar una linea del fichero de entrenamiento: {watch.ElapsedMilliseconds} ms");
                } while(!stepByStep && !_getEndOfLine());
            }
            catch(Exception)
            {
                foreach(var column in ListOfColumns)
                {
                    foreach(var layer in column.ListOfLayers)
                    {
                        layer.Dispose();
                    }
                }
            }
        }

        private void ManageTasksToReadInputs()
        {
            foreach (var column in ListOfColumns)
            {
                foreach (var layer in column.ListOfLayers)
                {
                    layer.PrepareTaskToReadInput();
                }
            }
            StartTasksToReadInputs();
            Task.WaitAll(ListOfTasksToReadInputs.ToArray());
            ListOfTasksToReadInputs.Clear();
        }

        private void ManageTasksToBuildStructures()
        {
            foreach (var column in ListOfColumns)
            {
                foreach (var layer in column.ListOfLayers)
                {
                    layer.PrepareTaskToBuildStructures();
                }
            }
            StartTasksToBuildStructures();
            Task.WaitAll(ListOfTasksToBuildStructures.ToArray());
            ListOfTasksToBuildStructures.Clear();
        }

        private void ManageTasksToGenerateOutputs()
        {
            foreach (var column in ListOfColumns)
            {
                foreach (var layer in column.ListOfLayers)
                {
                    layer.PrepareTaskToGenerateOutputs();
                }
            }
            StartTasksToGenerateOutputs();
            Task.WaitAll(ListOfTasksToGenerateOutputs.ToArray());
            ListOfTasksToGenerateOutputs.Clear();
        }

        private void StartTasksToGenerateOutputs()
        {
            foreach(var task in ListOfTasksToGenerateOutputs)
            {
                task.Start();
            }
        }

        private void StartTasksToBuildStructures()
        {
            foreach (var task in ListOfTasksToBuildStructures)
            {
                task.Start();
            }
        }

        private void StartTasksToReadInputs()
        {
            foreach (var task in ListOfTasksToReadInputs)
            {
                task.Start();
            }
        }

        //public void RunXudonSync(bool stepByStep = false)
        //{
        //    try
        //    {
        //        do
        //        {
        //            var watch = new Stopwatch();
        //            watch.Start();
        //            GetInputDataSync();
        //            //BuildStructureSync();
        //            SendOutputDataSync();
        //            watch.Stop();
        //            Debug.WriteLine($"Tiempo en procesar una linea del fichero de entrenamiento: {watch.ElapsedMilliseconds} ms");
        //        } while (!stepByStep && !_getEndOfLine());

        //        //foreach (var column in ListOfColumns)
        //        //{
        //        //    foreach (var layer in column.ListOfLayers)
        //        //    {
        //        //        GetInputDataSync();
        //        //        SendOutputDataSync();
        //        //    }
        //        //}
        //    }
        //    catch (Exception)
        //    {
        //        foreach (var column in ListOfColumns)
        //        {
        //            foreach (var layer in column.ListOfLayers)
        //            {
        //                layer.Dispose();
        //            }
        //        }
        //    }
        //}

        //private void BuildStructureThread()
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.BuildStructureThread();
        //    }
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.Thread.Join();
        //    }
        //}

        //private void BuildStructureSync()
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.BuildStructureSync();
        //    }
        //}

        //private void GetInputDataThread() //Diastole
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.GetInputDataThread();
        //    }
        //    foreach(var column in ListOfColumns)
        //    {
        //        column.Thread.Join();
        //    }
        //}

        //private void SendOutputDataThread() //Systole
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.SendOutputDataThread();
        //    }
        //    foreach(var column in ListOfColumns)
        //    {
        //        column.Thread.Join();
        //    }
        //}

        //private void GetInputDataSync() //Diastole
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.GetInputDataSync();
        //    }
        //}

        //private void SendOutputDataSync() //Systole
        //{
        //    foreach (var column in ListOfColumns)
        //    {
        //        column.SendOutputDataSync();
        //    }
        //}
    }
}
