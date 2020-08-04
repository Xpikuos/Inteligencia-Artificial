//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Common.Structure;

namespace XudonV4NetFramework.Structure
{
    public class DIFUSSORLayer : Layer
    {
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
        }

        public override void SendOutputDataSync() //Systole
        {
        }
    }
}
