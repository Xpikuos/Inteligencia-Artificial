using System;
using System.Collections.Generic;
using System.Text;

namespace XudonV4NetFramework.Common
{
    public static class HyperParameters
    {
        /// <summary>
        /// Carácter usado como separador de valores en el los ficheros de entrenamiento .csv
        /// </summary>
        public static char Separator { get; set; } = ',';

        /// <summary>
        /// Indica si las XCeldas van a usar una no-linealidad para calcular Aij
        /// </summary>
        public static bool UseNonLinearFunctionForAij { get; set; }

        /// <summary>
        /// Indica si la MicroRed va a usarse para aprender secuencias. Si es así LearnSequences=true y no se lleva a cabo una ordenación lexicográfica de los IDs
        /// </summary>
        public static bool LearnSequences { get; set; }

        /// <summary>
        /// W: Es un número entero positivo mayor que 0. Anchura de atomización permitida. Indica el número de entradas mínimo permitido que pueden tener las nuevas XCeldas-AND creadas después de una atomización. Gracias a este parámetro se fuerza a que la Red cree características más o menos complejas. Así, ‘W’ sería un indicador de ese nivel de complejidad mínimo requerido para formar una nueva característica (que es, al fin y al cabo, lo que representa una XCelda-AND).
        /// </summary>
        /// <value>
        /// The w.
        /// </value>
        public static uint W { get; set; } = 2;

        /// <summary>
        /// m: 0≤m≤1
        /// Es un número real.Está relacionado con el nivel de solapamiento permitido en los límites de las regiones de clasificación antes de considerar que se trata de una nueva clase.
        /// Controla el umbral a partir del cual se puede producir la fusión de XCeldas-FUZZY.
        /// </summary>
        public static double m { get; set; } = 0.1;

        /// <summary>
        /// R: R>0
        /// Es un número entero positivo. Define el número de regiones (de 0 a R-1) sobre las que se mapean las señales de entrada.
        /// Está relacionado, por tanto, con el nivel de precisión de la representación de los valores de entrada.
        /// Cuanto mayor sea el valor, mayor será el número de XCeldas que aparezcan en la Red.
        /// En este caso, este valor se usa para calcular alfa y determinar si una celda está activa o no, ya que,
        /// para las entradas, esta valor R se lee del fichero de configuración Pins.xml
        /// </summary>
        public static uint R { get; set; }

        /// <summary>
        /// r: 0≤r≤R/2
        /// Es un número real positivo.Define la anchura inicial de las funciones de pertencia de las XCeldas-FUZZY.Tiene un valor inicial típico de R/N (siendo N el número estimado de clases a clasificar), aunque puede ser mayor si se quiere hacer una clasificación más genérica.Incluso se puede hacer que r empieze con su máximo valor R/2 y que vaya decreciendo con cada iteración o con cada fragmentación. Esto podría favorecer un entrenamiento más rápido si los patrones están bien seleccionados y son muy representativos de cada clase.
        /// </summary>
        public static double r { get; set; } = 0.5;

        /// <summary>
        /// pi: 0≤pi≤1
        /// Es un número real.Umbral de probabilidad de los canales.Un canal se activa cuando Pijs=Nijs/(Nijs+Nijf)>pi. Con este valor pi se controla el nivel de certeza que se quiere obtener en el resultado de la mRed. Es, por tanto un parámetro global para toda la mRed. Durante el aprendizaje se puede forzar a tomar el valor 0.
        /// </summary>
        public static double pi { get; set; } = 0;

        /// <summary>
        /// Es un número real. Corresponde al umbral de activación de una XCelda. Una XCelda se activa cuando Aij≥alphai y sus canales de entrada estén activados (todos (en el caso de una XCelda-AND) o alguno de ellos (en el caso de una XCelda-OR)). Se calcula de forma dinámica a menos que se fuerce su valor externamente.
        /// Su valor podría disminuir si la XCeldai se activó en la iteración anterior.Esto haría que la XCelda fuese más fácilmente excitable aún cuando los canales de entrada tuvieran valores más bajos en la siguiente iteración.Esto estaría justificado biológicamente en el hecho de que los patrones no desaparecen instantáneamente, sino que siguen siendo observables por el sistema durante un cierto tiempo(si acaso alterados con pequeñas variaciones de escala y/o posición). Además, de esta forma, teóricamente, se conseguiría que la µRed fuese más insensible frente a traslaciones y se implementaría una cierta capacidad de aprendizaje de secuencias temporales condicionadas. (De todos modos, esto habría que simularlo)
        /// Si toma un valor distinto a null, fuerza el valor alphai de todas las XCeldas a tomar el valor de esta variable
        /// </summary>
        public static double? alpha { get; set; }

        /// <summary>
        /// n: n>0
        /// Es un número entero. Corresponde al umbral que limita la activación de la salida de las XCeldas-FUZZY con el fin de evitar patrones espurios. Cuanto mayor sea el valor, más difícilmente se activará la salida de las XCeldas-FUZZY y éstas sólo reaccionarán a patrones muy frecuentes y claros.Así, una XCelda-FUZZYi sólo podría generar una salida para un valor de entrada  ‘mi’ cuando Nmi>n. Si los datos de entrenamiento están correctamente elegidos, preprocesados y sin valores falsos, se puede hacer n=1. En caso contrario n=2 puede ser un buen valor.
        /// </summary>
        public static uint n { get; set; } = 1;

        /// <summary>
        /// nii: nii>0
        /// Es un número entero.Valor máximo al que se permite llegar a un contador a partir del cual se inicia el proceso del olvido dinámico. Este valor no debe ser muy grande ya que determina la frecuencia con la que se realiza el olvido dinámico y por tanto influye en la velocidad con la que una mRed aprende, así como en la cantidad de patrones necesarios para realizar el aprendizaje.  
        /// Un buen valor sería:
        /// nii = Número total de patrones en el fichero de entrenamiento/Número de clases esperadas a clasificar.
        /// Se puede pensar en hacer que este valor sea ajustable según la dificultad del patrón a aprender, que vaya decreciendo con cada iteración o que oscilase.
        /// </summary>
        public static uint nii { get; set; }
    }
}
