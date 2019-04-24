//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using XudonV2NetStandard.XCells;

namespace XudonV2NetStandard.Common
{
    public class Channel //Un canal modela una unión sináptica. Es decir una unión de un "axon terminal" con una dendrita
    {
        #region Private properties

        private Pin _pin;

        private AutoAlpha _autoAlpha;

        #endregion Private properties

        #region Public properties

        public static uint Resolution { get; set; }
        public static double Delta { get; set; } //Valor del error por debajo del cual se considera "aprendizaje correcto"

        public XCell XCellDestiny { get; set; }

        public XCell XCellOrigin { get; set; }

        public string PatternToSendToAnXCell { get; set; }

        public bool Inhibit { get; set; } //Inhibition channel
        public bool IsActive { get; set; } //Activation channel
        public int? Echo { get; set; } //Echo signal=1,0,-1, null
        public int Learn { get; set; } //Learn signal=1,-1, null (señal de aprendizaje)
        public double Wij { get; set; }
        public double Aij { get; set; }
        public ulong Nijpos { get; set; } //Npos
        public ulong Nijneg { get; set; } //Nneg
        public ulong Nijs { get; set; } //Nij success
        public ulong Nijf { get; set; } //Nij failure

        public Pin Pin //el Pin está sólo en uno de los extremos de un canal
        {
            get
            {
                return _pin;
            }
            set
            {
                _pin.Value += value.Value;
                _autoAlpha.GetMappedInputValueUpdateCountersAndLimits(_pin.Value);
                _autoAlpha.CalculateAlpha();
            }
        }

        #endregion Public properties

        #region Constructors
        public Channel(string id)
        {
            _autoAlpha = new AutoAlpha(Resolution);
            PatternToSendToAnXCell = string.Empty;
            Echo                   = null;
            XCellDestiny           = null;
            XCellOrigin            = null;
            IsActive               = false;
            Inhibit                = false;
            Nijpos                 = 1;
            Wij                    = 1;
            Aij                    = 0;
            Pin                    = new Pin(id,0);
        }

        public Channel(double value, string id)
        {
            _autoAlpha = new AutoAlpha(Resolution);
            PatternToSendToAnXCell = string.Empty;
            Echo                   = null;
            XCellDestiny           = null;
            XCellOrigin            = null;
            IsActive               = false;
            Inhibit                = false;
            Nijpos                 = 1;
            Wij                    = 1;
            Aij                    = 0;
            Pin                    = new Pin(id, value);
        }

        public Channel(Pin pin)
        {
            _autoAlpha = new AutoAlpha(Resolution);
            PatternToSendToAnXCell = string.Empty;
            Echo                   = null;
            XCellDestiny           = null;
            XCellOrigin            = null;
            IsActive               = false;
            Inhibit                = false;
            Nijpos                 = 1;
            Wij                    = 1;
            Aij                    = 0;
            Pin                    = pin;
        }

        #endregion Constructors

        #region Public methods

        public void ExecuteYourForwardFunctionality()
        {
            if(XCellDestiny == null && XCellOrigin != null && Pin != null) //Se ha llegado a la salida hacia el exterior
            {
                Echo = Math.Sign(XCellOrigin.OUT - Pin.Value);
                //ExecuteYourBackwardFunctionality();//Habrá que ver si es mejor hacerlo de forma ordenada
            }
            else if(XCellOrigin == null && Pin != null && XCellDestiny != null) //entrada desde el exterior
            {
                IsActive = true;
                Aij = Pin.Value;
                XCellDestiny.IN += Aij;
                XCellDestiny.ExecuteYourForwardFunctionality();
            }
            else if(XCellOrigin != null && XCellDestiny != null) //canal conectado a 2 XCeldas
            {
                Aij = Sigmoid(Convert.ToDouble(Nijpos - Nijneg) * Wij * XCellOrigin.OUT / Convert.ToDouble(XCellOrigin.Nii));
                if(Aij>=_autoAlpha.Alpha)
                {
                    IsActive = true;
                    XCellDestiny.IN = Aij;
                    XCellDestiny.ExecuteYourForwardFunctionality();
                }
                else
                {
                    IsActive = false;
                    Aij = 0;
                }
            }
        }

        public void ExecuteYourBackwardFunctionality()
        {
            if(IsActive)
            {
                UpdateCountersDependingOnEcho();
                XCellOrigin.ExecuteYourBackwardFunctionality();
                Pin.Value = 0;
                Aij = 0;
                IsActive = false;
            }
        }

        #endregion Public methods

        #region Private methods

        private void UpdateCountersDependingOnEcho()
        {
            if(Echo == null || !IsActive)
            {
                return;
            }

            var NijSign = Sign(Aij) * Sign(Echo);

            if(NijSign >= 0)
            {
                Nijpos++;
                XCellDestiny.Nii++;
            }
            else
            {
                Nijneg++;
                XCellDestiny.Nii++;
            }
        }

        private double Sigmoid(double value)
        {
            return 1 / (1 + Math.Exp(-(value-_autoAlpha.Alpha))); //desplazamos la sigmoide para que quede centrada en el valor umbral alpha
        }

        private int Sign(int? value)
        {
            if(value==null)
            {
                return 0;
            }

            if(value>=0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private int Sign(double value)
        {
            if(value >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        #endregion Private methods
    }
}
