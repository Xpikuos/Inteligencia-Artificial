//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System.Collections.Generic;
using System.Linq;
using XudonV2NetStandard.XCells;

namespace XudonV2NetStandard.Structure
{
    public class ANDLayer : Layer
    {
        public ICollection<XCellAND> ListOfXCellsAND { get; set; }

        public double Threshold { get; set; }

        public ANDLayer(uint layerNumber, double threshold=0): base (layerNumber)
        {
            Threshold = threshold;
            ListOfXCellsAND = new List<XCellAND>();
        }

        public string CreateIdOfAllActiveInputs()
        {
            var id = string.Empty;
            var listOfActiveInputs=ListOfInputChannels.Where(input => input.Pin.Value > Threshold);
            foreach (var activeInput in listOfActiveInputs)
            {
                id = $"{id}{activeInput.Pin.Id}|";
            }
            return id.TrimEnd('|');
        }

        public string CreateIdOfAllInActiveInputs()
        {
            var id = string.Empty;
            var listOfInActiveInputs = ListOfInputChannels.Where(input => input.Pin.Value < Threshold);
            foreach (var activeInput in listOfInActiveInputs)
            {
                id = $"{id}{activeInput.Pin.Id}|";
            }
            return id.TrimEnd('|');
        }

        public void CreateAnXCellANDGivenItsID(string id)
        {
            ListOfXCellsAND.Add(new XCellAND(id,this));
        }

        public override void MethodToExecuteAfterReadingAllInputData() //Diastole
        {

        }

        public override void MethodToExecuteAfterSendingAllInputData() //Systole
        {

        }
    }
}
