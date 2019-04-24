//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

namespace XudonV2NetStandard.Common
{
    /// <summary>
    /// Un Pin es cualquier punto al cual se le pueda asignar un Id y un valor. Típicamente son entradas o salidas.
    /// </summary>
    public class Pin
    {
        public string Id { get; set; }
        public double Value { get; set; }

        public Pin (string id)
        {
            Id = id;
        }

        public Pin(string id, double value)
        {
            Id = id;
            Value = value;
        }
    }
}
