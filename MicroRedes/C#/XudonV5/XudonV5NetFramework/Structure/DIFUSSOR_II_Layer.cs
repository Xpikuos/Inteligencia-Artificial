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
    public class DIFUSSOR_II_Layer : Layer
    {
        public ICollection<XCellDP_II> ListOfXCellsDPII { get; set; }

        public string PatternToSendInOutputChannels
        {
            get;
            private set;
        }

        public DIFUSSOR_II_Layer()
        {
            LayerName = "DP-II";
            PatternToSendInOutputChannels = string.Empty;
            ListOfXCellsDPII = new List<XCellDP_II>();
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsDPII.Cast<XCell>().ToList();
        }

        public List<string> GenerateIdsForConnectedOutputLinks(string incommingIdPattern, string idsOfConnectedOutputLinks)
        {
            var listOfIdsOfConnectedOutputLinks = new List<string>();

            if(incommingIdPattern == null || idsOfConnectedOutputLinks == null || incommingIdPattern.Length == 0 || idsOfConnectedOutputLinks.Length == 0)
            {
                return null;
            }
            else
            {
                var router = new Router();
                if(!idsOfConnectedOutputLinks.Contains("|"))
                {
                    var (restInIncommingPattern, restInPatternOfDestiny) = router.RemoveExistingIds(incommingIdPattern, idsOfConnectedOutputLinks);
                    listOfIdsOfConnectedOutputLinks.Add(restInIncommingPattern);
                }
                else
                {
                    foreach(var idOfConnectedOutputLinks in idsOfConnectedOutputLinks.Split('|'))
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
            foreach (var inputChannel in ListOfInputChannels)
            {
                if (inputChannel.XCellDestiny == null)
                {
                    var xCellDPII = new XCellDP_II(inputChannel.XCellOrigin.Id, this);
                    xCellDPII.ListOfInputChannels.Add(inputChannel);
                    inputChannel.XCellDestiny = xCellDPII;
                    ListOfXCellsDPII.Add(xCellDPII);
                }
            }

            foreach (var xCellDPII in ListOfXCellsDPII)
            {
                xCellDPII.GetInputData();
            }
        }

        public override void SendOutputDataSync() //Systole
        {
            foreach (var xCellDPII in ListOfXCellsDPII)
            {
                xCellDPII.SendOutputData();
            }

            foreach (var xCellDPII in ListOfXCellsDPII)
            {
                xCellDPII.ListOfInputChannels[0].PatternToSendToAnXCell = null;
                xCellDPII.ListOfInputChannels[0].Aij = double.NaN;
                xCellDPII.ListOfInputChannels[0].IsActive = false;
            }
        }
    }
}
