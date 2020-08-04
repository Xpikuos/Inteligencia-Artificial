//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.Structure;
using XudonV4NetFramework.XCells;
using System.Linq;

namespace XudonV4NetFramework.Structure
{
    public class REGRESSIONLayer : Layer
    {
        public ICollection<XCellSUM> ListOfXCellsSUM { get; set; }

        public REGRESSIONLayer()
        {
            LayerName = "REGRESSION";
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsSUM.Cast<XCell>().ToList();
        }

        public override void GetInputDataSync() //Diastole
        {
        }

        public override void SendOutputDataSync() //Systole
        {
        }
    }
}
