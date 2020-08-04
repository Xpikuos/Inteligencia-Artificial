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
    public class XCellAND : XCellLogic
    {
        public XCellAND(Layer layer) : base(layer) { }
        public XCellAND(string id, Layer layer) : base(id, layer) { }

        /// <summary>
        /// Autogénesis
        /// </summary>
        /// <param name="idSequence">id=id1&id2&...&idn</param>
        public void Autogenesis(string idSequence)
        {
            var ids = idSequence.Split('&');
            if(ids.Count() >= HyperParameters.W)
            {
                var newXCellAND = ((ANDLayer)Layer).CreateAnXCellANDGivenItsIDInTemporalList(idSequence);
                var outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND = new Channel();
                outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND.XCellOrigin = newXCellAND;
                Layer.ListOfOutputChannels.Add(outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND);
                newXCellAND.ListOfOutputChannels.Add(outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND);
                foreach(var id in ids)
                {
                    var inputChannel = Layer.ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == id);
                    if(inputChannel != null)
                    {
                        inputChannel.XCellDestiny = newXCellAND;
                        newXCellAND.ListOfInputChannels.Add(inputChannel);
                    }
                }

                newXCellAND.AssignLevel();
            }
        }

        /// <summary>
        /// Atomización
        /// </summary>
        /// <param name="idSequence">id=id1&id2&...&idn</param>
        public void Atomice(string idSequence)
        {
            var ids = idSequence.Split('&');
            if(ids.Count() >= HyperParameters.W)
            {
                var newXCellAND = ((ANDLayer)Layer).CreateAnXCellANDGivenItsIDInTemporalList(idSequence);
                var outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND = new Channel();
                outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND.XCellDestiny = this;
                outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND.XCellOrigin = newXCellAND;
                newXCellAND.ListOfOutputChannels.Add(outputChannelFornewXCeldaANDConnectedToInputOfOldXCellAND);
                foreach(var id in ids)
                {
                    var inputChannel=ListOfInputChannels.FirstOrDefault(channel => channel.XCellOrigin.Id == id);
                    if(inputChannel != null)
                    {
                        inputChannel.XCellDestiny = newXCellAND;
                        newXCellAND.ListOfInputChannels.Add(inputChannel);
                        ListOfInputChannels.Remove(inputChannel); //Al eliminar este inputChannel de la lista, se elimina tb el inputChannel???
                    }
                }

                newXCellAND.AssignLevel();
                ListOfInputChannels.Add(newXCellAND.ListOfOutputChannels[0]);
            }
        }

        public override void ActivateOutputChannelsAndGenerateOutputValue()
        {
            var outputValue = double.MinValue;
            foreach (var inputChannelActive in ListOfInputChannels.Where(inputChannel => inputChannel.IsActive))
            {
                var value = (inputChannelActive.Nijpos - inputChannelActive.Nijneg) * inputChannelActive.Aij / Nii;
                if (value > outputValue) { outputValue = value; }
            }

            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.Aij = outputValue;
                outputChannel.IsActive = outputChannel.Aij >= 0; //el caso =0 es para asegurar la difusión (maximiza la entropía en caso de duda)
            }
        }

        public override void AssignInputDependingOnXCellType(double value)
        {
            if(value < IN) //Criterio como AND fuzzy: se toma el menor
            {
                IN = value;
            }
        }

        public override void AssignLevel()
        {
            var Lj = double.MaxValue;
            var Lh = 0.0;

            if(ListOfInputChannels.Count()==0)
            {
                Lh = Layer.LayerDown.LayerNumber;
            }
            else
            {
                foreach (var inputChannel in ListOfInputChannels)
                {
                    var LhTemp = 0.0;
                    if (inputChannel.XCellOrigin == null)
                    {
                        LhTemp = Layer.LayerNumber - 1;
                    }
                    else
                    {
                        LhTemp = inputChannel.XCellOrigin.Li;
                    }
                    if (LhTemp > Lh)
                    {
                        Lh = LhTemp;
                    }
                }
            }

            if(ListOfOutputChannels.Count==0)
            {
                Lj = Layer.LayerUp.LayerNumber;
            }
            else
            {
                foreach (var outputChannel in ListOfOutputChannels)
                {
                    var LjTemp = 0.0;
                    if (outputChannel.XCellDestiny == null)
                    {
                        LjTemp = Layer.LayerNumber + 1;
                    }
                    else
                    {
                        LjTemp = outputChannel.XCellDestiny.Li;
                    }
                    if (LjTemp < Lj)
                    {
                        Lj = LjTemp;
                    }
                }
            }

            Li = (Lj+Lh)/2;
        }

        /// <summary>
        /// Hacer la atomización o la autogénesis al leer los datos de entrada para generar la estructura que se usará para hacer SendOutputData
        /// </summary>
        public override void GetInputData() //Diastole
        {
            var splittedIds = Id.Split('&');
            var concatenatedRestingIDs = string.Empty;
            var idsOfActiveChannels = string.Empty;

            var listOfActiveChannels = ListOfInputChannels.Where(inputChannel => inputChannel.IsActive).ToList();

            foreach (var inputChannel in listOfActiveChannels)
            {
                var patternToSendToAnXCell = inputChannel.PatternToSendToAnXCell;

                foreach (var splittedId in splittedIds)
                {
                    inputChannel.PatternToSendToAnXCell = inputChannel.PatternToSendToAnXCell.Trim(splittedId.ToCharArray()).Replace("&&", string.Empty);
                }
                if (!string.IsNullOrEmpty(inputChannel.PatternToSendToAnXCell) && !string.IsNullOrEmpty(concatenatedRestingIDs))
                {
                    concatenatedRestingIDs = $"{concatenatedRestingIDs}{inputChannel.PatternToSendToAnXCell}&";
                }

                if(inputChannel.IsActive && !string.IsNullOrEmpty(inputChannel.XCellOrigin.Id) && !string.IsNullOrEmpty(idsOfActiveChannels))
                {
                    idsOfActiveChannels = $"{idsOfActiveChannels}{inputChannel.XCellOrigin.Id}&";
                }
            }
            concatenatedRestingIDs = concatenatedRestingIDs.TrimEnd('&');
            idsOfActiveChannels = idsOfActiveChannels.TrimEnd('&');

            if (listOfActiveChannels.Count() == ListOfInputChannels.Count()) //Han llegado patrones por todos los canales
            {
                if(concatenatedRestingIDs.Length==0) //Se han eliminado todos los IDs en los InP
                {
                    var kk = 0;
                    //AuxPattern = string.Empty;
                    //AuxIsActive = false;
                    //double minValue = double.MaxValue;
                    //foreach (var inputChannel in ListOfInputChannels)
                    //{
                    //    if (inputChannel.IsActive)
                    //    {
                    //        AuxIsActive = true;
                    //        if (inputChannel.Aij < minValue)
                    //        {
                    //            minValue = inputChannel.Aij;
                    //        }
                    //        if (!string.IsNullOrEmpty(inputChannel.PatternToSendToAnXCell))
                    //        {
                    //            AuxPattern = $"{AuxPattern}{inputChannel.PatternToSendToAnXCell}|";
                    //        }
                    //    }
                    //}
                    //AuxPattern = AuxPattern.TrimEnd('|');
                    //IN = minValue;
                }
                else //Quedan IDs en los InP
                {
                    var splittedConcatenatedRestingIDs = concatenatedRestingIDs.Split('&');
                    var messageWasSent = false;
                    foreach (var outputChannel in ListOfOutputChannels)
                    {
                        foreach(var restingID in splittedConcatenatedRestingIDs)
                        {
                            if (outputChannel.XCellDestiny.Id.Contains(restingID))
                            {
                                //calcular la AND de los valores de entrada y dejar el valor preparado
                                //IN=
                                //foreach
                                messageWasSent = true;
                            }
                        }
                    }
                    if(!messageWasSent)
                    {
                        var kk = 0;
                        //Autogenesis(concatenatedRestingIDs);
                    }
                }
            }
            else //Han llegado patrones sólo por algunos canales
            {
                var kk = 0;
                //Atomice(idsOfActiveChannels);
            }

        }

        public override void SendOutputData() //Systole
        {
            foreach (var outputChannel in ListOfOutputChannels)
            {
                outputChannel.PatternToSendToAnXCell = AuxPattern;
                outputChannel.IsActive = AuxIsActive;
                outputChannel.Aij = IN;
            }

            foreach (var inputChannel in ListOfInputChannels)
            {
                inputChannel.PatternToSendToAnXCell = null;
                inputChannel.IsActive = false;
                inputChannel.Aij = double.NaN;
            }
        }
    }
}
