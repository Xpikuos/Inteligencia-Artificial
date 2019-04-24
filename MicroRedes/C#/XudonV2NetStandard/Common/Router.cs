//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

namespace XudonV2NetStandard.Common.Structure
{
    public class Router
    {
        /// <summary>
        /// Suponemos que los patrones están ordenados lexicográficamente
        /// </summary>
        /// <param name="incommingIdPattern"></param>
        /// <param name="destinyIdPattern"></param>
        /// <returns></returns>
        public (string restInIncommingIdPattern, string restInDestinyIdPattern) RemoveExistingIds(string incommingIdPattern, string destinyIdPattern)
        {
            if(destinyIdPattern == null || incommingIdPattern == null || destinyIdPattern.Length == 0 || incommingIdPattern.Length == 0)
            {
                return (incommingIdPattern, destinyIdPattern);
            }

            if(destinyIdPattern.Length > incommingIdPattern.Length)
            {
                if(!incommingIdPattern.Contains("|"))
                {
                    if(destinyIdPattern.Contains(incommingIdPattern))
                    {
                        destinyIdPattern = destinyIdPattern.Replace(incommingIdPattern, string.Empty);
                        incommingIdPattern = incommingIdPattern.Replace(incommingIdPattern, string.Empty);
                    }
                }
                else
                {
                    foreach(var splittedIncommingPatern in incommingIdPattern.Split('|'))
                    {
                        if(destinyIdPattern.Contains(splittedIncommingPatern))
                        {
                            destinyIdPattern = destinyIdPattern.Replace(splittedIncommingPatern, string.Empty);
                            incommingIdPattern = incommingIdPattern.Replace(splittedIncommingPatern, string.Empty);
                        }
                    }
                }
            }
            else
            {
                if(!destinyIdPattern.Contains("|"))
                {
                    if(incommingIdPattern.Contains(destinyIdPattern))
                    {
                        incommingIdPattern = incommingIdPattern.Replace(destinyIdPattern, string.Empty);
                        destinyIdPattern = destinyIdPattern.Replace(destinyIdPattern, string.Empty);
                    }
                }
                else
                {
                    foreach(var splittedPatternOfDestiny in destinyIdPattern.Split('|'))
                    {
                        if(incommingIdPattern.Contains(splittedPatternOfDestiny))
                        {
                            destinyIdPattern = destinyIdPattern.Replace(splittedPatternOfDestiny, string.Empty);
                            incommingIdPattern = incommingIdPattern.Replace(splittedPatternOfDestiny, string.Empty);
                        }
                    }
                }
            }

            destinyIdPattern = destinyIdPattern.TrimEnd('|').TrimStart('|').Replace("||", "|");
            incommingIdPattern = incommingIdPattern.TrimEnd('|').TrimStart('|').Replace("||", "|");

            return (incommingIdPattern, destinyIdPattern);
        }
    }
}
