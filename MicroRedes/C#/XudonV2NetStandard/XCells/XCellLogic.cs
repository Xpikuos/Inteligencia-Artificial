﻿//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using XudonV2NetStandard.Structure;

namespace XudonV2NetStandard.XCells
{
    public class XCellLogic : XCell
    {
        public XCellLogic(Layer layer):base (layer) { }
        public XCellLogic(string id, Layer layer) :base (id, layer) { }
    }
}
