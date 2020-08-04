//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.XCells;
using System.Linq;

namespace XudonV4NetFramework.Structure
{
    public class ORExplodeLayer : Layer
    {
        private bool _newPattern;

        public new ICollection<XCellORExplode> ListOfXCellsOrExplode { get; set; }

        public ORExplodeLayer()
        {
            LayerName = "OR-EXPLODE";
            ListOfXCellsOrExplode = new List<XCellORExplode>();
        }

        public void GenerateNewPattern()
        {
            //TODO
            _newPattern = true;
        }
        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsOrExplode.Cast<XCell>().ToList();
        }


        public override void GetInputDataSync() //Diastole
        {
            foreach(var xCellOrExpl in ListOfXCellsOrExplode)
            {
                xCellOrExpl.GetInputData();
            }
        }

        public override void SendOutputDataSync() //Systole
        {
            foreach (var xCellOrExpl in ListOfXCellsOrExplode)
            {
                xCellOrExpl.SendOutputData();
            }
        }

        public override void ConnectThisLayerWithOutputLayer(Layer outputLayer)
        {
            LayerNumber = (outputLayer.LayerNumber+outputLayer.LayerUp.LayerNumber)/2;
            LayerUp = outputLayer;
        }
    }
}
