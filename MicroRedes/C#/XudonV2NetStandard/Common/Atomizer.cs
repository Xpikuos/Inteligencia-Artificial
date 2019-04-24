//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita

//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es
using System.Collections.Generic;
using System.Linq;

namespace XudonV2NetStandard.Common.Structure
{
    public class Atomizer
    {
        private const int MIN_ATOMIZATION_LEVEL = 1;

        /// <summary>
        /// Se supone que bigString y shortString están lexicográficamente ordenadas
        /// </summary>
        /// <param name="bigIDString">ID de una XCelda</param>
        /// <param name="shortIDString">IDs concatenados de las entradas activas</param>
        /// <param name="atomizationLevel"></param>
        /// <returns>Devuelve el ID de las nuevas celdas que hay que crear, o una lista vacía si no hay que crear ninguna celda nueva</returns>
        public static ICollection<string> Atomize(string bigIDString, string shortIDString, int atomizationLevel=1)
        {
            ICollection<string> listOfAtomizedIDStrings = new List<string>();
            if (bigIDString == shortIDString) return listOfAtomizedIDStrings;

            var listOfStringsInBigIDString   = bigIDString.Split('|');
            var listOfStringsInShortIDString = shortIDString.Split('|');
            var stringsFound                 = string.Empty;
            var stringsNotFound              = string.Empty;
            int numOfStringsFound            = 0;
            int numOfStringsNotFound         = 0;

            foreach (var stringInBigIDString in listOfStringsInBigIDString)
            {
                if (listOfStringsInShortIDString.Any(found => found == stringInBigIDString))
                {
                    stringsFound = $"{stringsFound}{stringInBigIDString}|";
                    numOfStringsFound++;
                }
                else
                {
                    stringsNotFound = $"{stringsNotFound}{stringInBigIDString}|";
                    numOfStringsNotFound++;
                }
            }

            if (numOfStringsFound == MIN_ATOMIZATION_LEVEL || numOfStringsNotFound == MIN_ATOMIZATION_LEVEL
                || numOfStringsFound <= atomizationLevel || numOfStringsNotFound <= atomizationLevel)
            {
                return listOfAtomizedIDStrings;
            }

            if (stringsFound    != string.Empty) listOfAtomizedIDStrings.Add(stringsFound.TrimEnd('|'));
            if (stringsNotFound != string.Empty) listOfAtomizedIDStrings.Add(stringsNotFound.TrimEnd('|'));

            return listOfAtomizedIDStrings;
        }
    }
}
