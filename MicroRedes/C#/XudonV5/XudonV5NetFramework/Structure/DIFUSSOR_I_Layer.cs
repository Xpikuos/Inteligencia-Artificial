//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Common.Structure;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Structure
{
    public class DIFUSSOR_I_Layer : Layer
    {
        public ICollection<XCellDP_I> ListOfXCellsDPI { get; set; }

        public string PatternToSendInOutputChannels
        {
            get;
            private set;
        }

        public DIFUSSOR_I_Layer()
        {
            LayerName = "DP-I";
            PatternToSendInOutputChannels = string.Empty;
            ListOfXCellsDPI = new List<XCellDP_I>();
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsDPI.Cast<XCell>().ToList();
        }

        public List<string> GenerateIdsForConnectedOutputLinks(string incommingIdPattern, string idsOfConnectedOutputLinks)
        {
            var listOfIdsOfConnectedOutputLinks = new List<string>();

            if (incommingIdPattern == null || idsOfConnectedOutputLinks == null || incommingIdPattern.Length == 0 || idsOfConnectedOutputLinks.Length == 0)
            {
                return null;
            }
            else
            {
                var router = new Router();
                if (!idsOfConnectedOutputLinks.Contains("&"))
                {
                    var (restInIncommingPattern, restInPatternOfDestiny) = router.RemoveExistingIds(incommingIdPattern, idsOfConnectedOutputLinks);
                    listOfIdsOfConnectedOutputLinks.Add(restInIncommingPattern);
                }
                else
                {
                    foreach (var idOfConnectedOutputLinks in idsOfConnectedOutputLinks.Split('&'))
                    {
                        var (restInIncommingPattern, restInPatternOfDestiny) = router.RemoveExistingIds(incommingIdPattern, idOfConnectedOutputLinks);
                        listOfIdsOfConnectedOutputLinks.Add(restInIncommingPattern);
                    }
                }
            }

            return listOfIdsOfConnectedOutputLinks;
        }

        public override void GetInputDataSync() //Diastole
        {
            PatternToSendInOutputChannels = string.Empty;

            var activeChannels = ListOfInputChannels.Where(inputChannel => inputChannel.IsActive).OrderBy(inputChannel => inputChannel.XCellOrigin.Id).ToList();
            var activeChannelsWithoutXCellDestiny = activeChannels.Where(activeChannel => activeChannel.XCellDestiny == null).ToList();

            foreach (var activeChannelWithoutXCellDestiny in activeChannelsWithoutXCellDestiny)
            {
                var xCellDP_I = new XCellDP_I(activeChannelWithoutXCellDestiny.XCellOrigin.Id, this);
                activeChannelWithoutXCellDestiny.XCellDestiny = xCellDP_I;
                xCellDP_I.ListOfInputChannels.Add(activeChannelWithoutXCellDestiny);
                ListOfXCellsDPI.Add(xCellDP_I);
            }

            foreach (var xCellDPI in ListOfXCellsDPI)
            {
                xCellDPI.GetInputData();
                xCellDPI.SendOutputData();
            }

            foreach (var xCellDPI in ListOfXCellsDPI.Where(xCelldpI => xCelldpI.ListOfOutputChannels[0].IsActive).OrderBy(xCelldpI => xCelldpI.ListOfOutputChannels[0].PatternToSendToAnXCell).ToList())
            {
                PatternToSendInOutputChannels = $"{PatternToSendInOutputChannels}{xCellDPI.ListOfOutputChannels[0].PatternToSendToAnXCell}&";
            }
            PatternToSendInOutputChannels = PatternToSendInOutputChannels.TrimEnd('&');

            var c1 = activeChannels.Count();
            var c2 = activeChannelsWithoutXCellDestiny.Count();

            if (c1 == c2 && c1 > 0)
            {
                if (!string.IsNullOrEmpty(PatternToSendInOutputChannels))
                {
                    var xCellAND = ((ANDLayer)LayerUp).CreateAnXCellANDGivenItsID(PatternToSendInOutputChannels);
                    //Crear una XCellAND con los canales de entrada igual a los canales de salida de las XCellDP_I creadas
                    foreach (var activeChannelWithoutXCell in activeChannelsWithoutXCellDestiny)
                    {
                        xCellAND.ListOfInputChannels.Add(activeChannelWithoutXCell.XCellDestiny.ListOfOutputChannels[0]);
                    }
                }
            }

            foreach (var xCellDPI in ListOfXCellsDPI)
            {
                xCellDPI.GetInputData();
            }

            foreach (var xCellDPI in ListOfXCellsDPI)
            {
                xCellDPI.ListOfInputChannels[0].PatternToSendToAnXCell = null;
                xCellDPI.ListOfInputChannels[0].Aij = double.NaN;
                xCellDPI.ListOfInputChannels[0].IsActive = false;
            }
        }

        //public override void GetInputDataSync() //Diastole
        //{
        //    PatternToSendInOutputChannels = string.Empty;

        //    var activeChannels = ListOfInputChannels.Where(inputChannel => inputChannel.IsActive).OrderBy(inputChannel => inputChannel.XCellOrigin.Id).ToList();
        //    var activeChannelsWithoutXCellDestiny = activeChannels.Where(activeChannel => activeChannel.XCellDestiny == null).ToList();

        //    //if (activeChannels != null)
        //    //{
        //    //    foreach (var activeChannel in activeChannels)
        //    //    {
        //    //        PatternToSendInOutputChannels = $"{PatternToSendInOutputChannels}{activeChannel.PatternToSendToAnXCell}&";
        //    //    }
        //    //    PatternToSendInOutputChannels = PatternToSendInOutputChannels.TrimEnd('&');
        //    //}

        //    foreach (var activeChannelWithoutXCellDestiny in activeChannelsWithoutXCellDestiny)
        //    {
        //        var xCellDP_I = new XCellDP_I(activeChannelWithoutXCellDestiny.XCellOrigin.Id, this);
        //        activeChannelWithoutXCellDestiny.XCellDestiny = xCellDP_I;
        //        xCellDP_I.ListOfInputChannels.Add(activeChannelWithoutXCellDestiny);
        //        ListOfXCellsDPI.Add(xCellDP_I);
        //    }

        //    foreach (var xCellDPI in ListOfXCellsDPI)
        //    {
        //        xCellDPI.GetInputData();
        //        xCellDPI.SendOutputData();
        //    }

        //    foreach (var xCellDPI in ListOfXCellsDPI.Where(xCelldpI => xCelldpI.ListOfOutputChannels[0].IsActive).OrderBy(xCelldpI => xCelldpI.ListOfOutputChannels[0].PatternToSendToAnXCell).ToList())
        //    {
        //        PatternToSendInOutputChannels = $"{PatternToSendInOutputChannels}{xCellDPI.ListOfOutputChannels[0].PatternToSendToAnXCell}&";
        //        //xCellDPI.ListOfOutputChannels[0].PatternToSendToAnXCell = null;
        //        //xCellDPI.ListOfOutputChannels[0].Aij = double.NaN;
        //        //xCellDPI.ListOfOutputChannels[0].IsActive = false;
        //    }
        //    PatternToSendInOutputChannels = PatternToSendInOutputChannels.TrimEnd('&');

        //    //var c1 = activeChannels.Count();
        //    //var c2 = activeChannelsWithoutXCellDestiny.Count();

        //    //if (c1 == c2 && c1>0)
        //    //{
        //    //    if(!string.IsNullOrEmpty(PatternToSendInOutputChannels))
        //    //    {
        //    //        var xCellAND = ((ANDLayer)LayerUp).CreateAnXCellANDGivenItsID(PatternToSendInOutputChannels);
        //    //        //Crear una XCellAND con los canales de entrada igual a los canales de salida de las XCellDP_I creadas
        //    //        foreach (var activeChannelWithoutXCell in activeChannelsWithoutXCellDestiny)
        //    //        {
        //    //            xCellAND.ListOfInputChannels.Add(activeChannelWithoutXCell.XCellDestiny.ListOfOutputChannels[0]);
        //    //        }
        //    //    }
        //    //}

        //    //foreach(var xCellDPI in ListOfXCellsDPI)
        //    //{
        //    //    xCellDPI.GetInputData();
        //    //}

        //    //foreach (var xCellDPI in ListOfXCellsDPI)
        //    //{
        //    //    xCellDPI.ListOfInputChannels[0].PatternToSendToAnXCell = null;
        //    //    xCellDPI.ListOfInputChannels[0].Aij = double.NaN;
        //    //    xCellDPI.ListOfInputChannels[0].IsActive = false;
        //    //}
        //}

        public override void SendOutputDataSync() //Systole
        {
            //foreach (var outputChannel in ListOfOutputChannels.OrderBy(inputChannel => inputChannel.XCellOrigin.Id))
            //{
            //    foreach (var channelToXCellAnd in outputChannel.XCellDestiny.ListOfOutputChannels)
            //    {
            //        channelToXCellAnd.PatternToSendToAnXCell = PatternToSendInOutputChannels;
            //    }
            //}

            foreach (var xCellDPI in ListOfXCellsDPI.OrderBy(xCellDpI=> xCellDpI.Id).ToList())
            {
                xCellDPI.SendOutputData();
            }


            foreach (var xCellDPI in ListOfXCellsDPI)
            {
                xCellDPI.ListOfInputChannels[0].PatternToSendToAnXCell = null;
                xCellDPI.ListOfInputChannels[0].Aij = double.NaN;
                xCellDPI.ListOfInputChannels[0].IsActive = false;
            }
        }
    }
}
