//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using System.Collections.Generic;
using System.Linq;
using XudonV4NetFramework.Common;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Structure
{
    public class ORLayer : Layer
    {
        public ICollection<XCellOR> ListOfXCellsOR { get; set; }

        private bool _newPattern;

        private Func<string> GetLastLineReadInDataFile; //used to read the last read line with values of the training file
        private List<string> _outputHeadersIDs;

        public ORLayer(Func<string> getLastLineReadInDataFile, List<string> outputHeadersIDs)
        {
            //_inputLayer.LineWithValues
            GetLastLineReadInDataFile = getLastLineReadInDataFile;
            _outputHeadersIDs = outputHeadersIDs;
            LayerName = "OR";
            ListOfXCellsOR = new List<XCellOR>();
            foreach(var outputHeaderID in _outputHeadersIDs)
            {
                var xCellOR = new XCellOR(outputHeaderID, this);
                ListOfXCellsOR.Add(xCellOR);
            }
        }

        public void GenerateNewPattern()
        {
            //TODO
            _newPattern = true;
        }

        public override ICollection<XCell> GetListOfXCells()
        {
            return ListOfXCellsOR?.Cast<XCell>()?.ToList();
        }

        public override void GetInputDataSync() //Diastole
        {
            if(ListOfXCellsOR!=null)
            {
                foreach (var xcellOR in ListOfXCellsOR)
                {
                    xcellOR.SendOutputData();
                }
            }
        }

        public override void SendOutputDataSync() //Systole
        {
        }
    }
}
