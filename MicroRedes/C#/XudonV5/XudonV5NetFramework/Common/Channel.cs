//Título: Microredes(mRedes)
//Subtítulo: V3.1.3.2
//Autor: Amro Xpike(propietario del canal de Youtube “Xpikuos”)
//Licencia:
//Este trabajo está licenciado bajo la licencia de Atribución-NoComercial-CompartirIgual 4.0 Internacional(CC BY-NC-SA 4.0)
//Para ver una copia de esta licencia, visita
//https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es

using System;
using XudonV4NetFramework.XCells;

namespace XudonV4NetFramework.Common
{
    public class Channel //Un canal modela una unión sináptica. Es decir una unión de un "axon terminal" con una dendrita
    {
        #region Public properties

        public static double Delta { get; set; } //Valor del error por debajo del cual se considera "aprendizaje correcto"

        public XCell XCellDestiny { get; set; }

        public XCell XCellOrigin { get; set; }

        public string PatternToSendToAnXCell { get; set; }

        public bool IsActive { get; set; } //Activation channel
        public int? Echo { get; set; } //Echo signal=1,0,-1, null
        public int Learn { get; set; } //Learn signal=1,-1, null (señal de aprendizaje)
        public double Wij { get; set; }
        public double Aij { get; set; }
        public ulong Nijpos { get; set; } //Npos
        public ulong Nijneg { get; set; } //Nneg
        public ulong Nijs { get; set; } //Nij success
        public ulong Nijf { get; set; } //Nij failure

        #endregion Public properties

        #region Constructors
        public Channel()
        {
            Echo = null;
            XCellDestiny = null;
            XCellOrigin = null;
            IsActive = false;
            Nijpos = 1;
            Wij = 1;
            Aij = 0;
        }
        //public Channel(Action <Channel> buildStructureJustBeforeSendingData,string patternToSendToAnXCell)
        //{
        //    _buildStructureJustBeforeSendingData = buildStructureJustBeforeSendingData;
        //    _autoAlpha = new AutoAlpha();
        //    PatternToSendToAnXCell = patternToSendToAnXCell;
        //    Echo = null;
        //    XCellDestiny = null;
        //    XCellOrigin = null;
        //    IsActive = false;
        //    Nijpos = 1;
        //    Wij = 1;
        //    _aij = 0;
        //}

        //public Channel(double value)
        //{
        //    _autoAlpha = new AutoAlpha();
        //    PatternToSendToAnXCell = string.Empty;
        //    Echo                   = null;
        //    XCellDestiny           = null;
        //    XCellOrigin            = null;
        //    IsActive               = false;
        //    Nijpos                 = 1;
        //    Wij                    = 1;
        //    Aij                    = 0;
        //}

        #endregion Constructors

        #region Public methods

        //public void TryToBuildStructureJustAfterAssignValue()
        //{
        //    if(XCellDestiny == null && IsActive && !string.IsNullOrEmpty(PatternToSendToAnXCell))
        //    {
        //        _buildStructureJustBeforeSendingData(this);
        //        PatternToSendToAnXCell = null; //clean the pattern for the next time. In general the pattern will be used only to create new structures
        //    }
        //}

        public void ExecuteYourForwardFunctionality()
        {
            //if(XCellDestiny == null && XCellOrigin != null)// && Pin != null) //Se ha llegado a la salida hacia el exterior
            //{
            //    Echo = Math.Sign(XCellOrigin.OUT - Aij);
            //    //ExecuteYourBackwardFunctionality();//Habrá que ver si es mejor hacerlo de forma ordenada
            //}
            //else if(XCellOrigin == null && XCellDestiny != null)//&& Pin != null //entrada desde el exterior
            //{
            //    IsActive = true;
            //    XCellDestiny.IN += Aij;
            //    XCellDestiny.ExecuteYourForwardFunctionality();
            //}
            //else if(XCellOrigin != null && XCellDestiny != null) //canal conectado a 2 XCeldas

            if (XCellOrigin != null && XCellDestiny != null) //canal conectado a 2 XCeldas
            {
                var Pijs = Convert.ToDouble(Nijs) / Convert.ToDouble(Nijs + Nijf);
                if (Pijs>= HyperParameters.pi || !string.IsNullOrEmpty(PatternToSendToAnXCell))
                {
                    IsActive = true;
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
                Aij = 0;
                IsActive = false;
            }
        }

        #endregion Public methods

        #region Private methods

        private int Sign(int? value)
        {
            if (value == null)
            {
                return 0;
            }

            if (value >= 0)
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
            if (value >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

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
        
        #endregion Private methods
    }
}
