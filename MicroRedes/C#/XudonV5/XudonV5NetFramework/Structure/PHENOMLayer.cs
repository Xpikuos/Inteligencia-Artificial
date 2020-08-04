//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using XudonV4NetFramework.Common;

namespace XudonV4NetFramework.Structure
{
    public class PHENOMLayer : Layer
    {
        //public new List<XCellPhenom> ListOfXCells { get; set; }

        /// <summary>
        /// Create a PHENOMLayer with an specific resolution for the input values
        /// </summary>
        public PHENOMLayer()
        {
            LayerName = "PHENOM";
        }

        public override void GetInputDataSync() //Diastole
        {
        }

        public override void SendOutputDataSync() //Systole
        {
        }
    }
}
