//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

namespace XudonV2NetStandard.Structure
{
    public class PHENOMLayer : Layer
    {
        private uint _resolution;

        /// <summary>
        /// Create a PHENOMLayer with an specific resolution for the input values
        /// </summary>
        /// <param name="resolution">From 0 to Number of steps</param>
        public PHENOMLayer(uint layerNumber, uint resolution) : base(layerNumber)
        {
            _resolution = resolution;
        }

        public override void MethodToExecuteAfterReadingAllInputData() //Diastole
        {

        }

        public override void MethodToExecuteAfterSendingAllInputData() //Systole
        {

        }
    }
}
