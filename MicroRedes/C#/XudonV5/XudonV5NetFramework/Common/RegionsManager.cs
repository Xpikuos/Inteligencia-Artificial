using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XudonV4NetFramework.Common
{
    public class RegionsManager
    {
        /// <summary>
        /// Lista de regiones (límite inferior de la base de una función de pertenencia, límite superior de la base de esa función de pertenencia)
        /// </summary>
        public List<(double lowerLimit, double upperLimit)> ListOfRegions;

        public double r { get; set; }
        public uint R { get; set; }

        public RegionsManager(uint _R)
        {
            R = _R;
            r = Convert.ToDouble(R) / 2;
            ListOfRegions = new List<(double, double)>();
        }

        public double Recalculate_r()
        {
            r = GetMaxWidthOfFreeRegions() / 2;
            return r;
        }

        public bool CheckIfRegionIsAvailable(double lowerLimit, double upperLimit)
        {
            var upperOverlappingFound = ListOfRegions.Any(element => element.upperLimit > lowerLimit);
            var lowerOverlappingFound = ListOfRegions.Any(element => element.lowerLimit > upperLimit);
            return !(lowerOverlappingFound || upperOverlappingFound);
        }

        public bool TryToAddANewRegion(double lowerLimit, double upperLimit)
        {
            if (CheckIfRegionIsAvailable(lowerLimit, upperLimit))
            {
                ListOfRegions.Add((lowerLimit, upperLimit));
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool TryToAddANewRegion(double inputValue, double r, out double lowerLimit, out double upperLimit)
        //{
        //    var regionFound = CheckIfValueIsInSideAnyRegion(inputValue);
        //    if (regionFound.lowerLimit!=default(double) && regionFound.upperLimit!=default(double))
        //    {
        //        lowerLimit = regionFound.lowerLimit;
        //        upperLimit = regionFound.upperLimit;
        //        if ((inputValue + r) > regionFound.upperLimit)
        //        {
        //            regionFound.upperLimit = inputValue + r;
        //            upperLimit = regionFound.upperLimit;
        //        }
        //        if ((inputValue - r) < regionFound.lowerLimit)
        //        {
        //            regionFound.lowerLimit = inputValue - r;
        //            lowerLimit = regionFound.lowerLimit;
        //        }
        //        return true;
        //    }

        //    if(CheckIfRegionIsAvailable(lowerLimit, upperLimit))
        //    {
        //        ListOfRegions.Add((lowerLimit, upperLimit));
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private (double lowerLimit, double upperLimit) CheckIfValueIsInSideAnyRegion(double inputValue)
        {
            return ListOfRegions.FirstOrDefault(region => region.lowerLimit >= inputValue && region.upperLimit <= inputValue);
        }

        public bool TryToExpandARegion(double lowerLimit, double upperLimit)
        {
            var meanValue = (lowerLimit + upperLimit) / 2;
            var regionWhereTheLowerLimitIs = ListOfRegions.Where(element => element.lowerLimit <= lowerLimit && element.upperLimit >= lowerLimit);
            var regionWhereTheUpperLimitIs = ListOfRegions.Where(element => element.lowerLimit <= upperLimit && element.upperLimit >= upperLimit);
            if(regionWhereTheLowerLimitIs == null && regionWhereTheUpperLimitIs == null)
            {
                ListOfRegions.Add((lowerLimit, upperLimit));
                return true;
            }
            else if(regionWhereTheLowerLimitIs!=null && regionWhereTheUpperLimitIs == null)
            {
                if(CheckIfValueIsInsideARegion(regionWhereTheLowerLimitIs.ToList()[0], meanValue))
                {
                    regionWhereTheLowerLimitIs.ToList()[0] = (regionWhereTheLowerLimitIs.ElementAt(0).lowerLimit, upperLimit);
                    return true;
                }
                return false;
            }
            else if(regionWhereTheLowerLimitIs == null && regionWhereTheUpperLimitIs != null)
            {
                if(CheckIfValueIsInsideARegion(regionWhereTheUpperLimitIs.ToList()[0], meanValue))
                {
                    regionWhereTheUpperLimitIs.ToList()[0] = (lowerLimit, regionWhereTheUpperLimitIs.ElementAt(0).upperLimit);
                    return true;
                }
                return false;
            }
            else if(regionWhereTheLowerLimitIs != null && regionWhereTheUpperLimitIs != null)
            {
                //TODO: Qué pasa si los límites de la nueva región caen, cada uno de ellos, dentro de regiones diferentes
                //if(regionWhereTheLowerLimitIs.ElementAt(0).upperLimit==regionWhereTheUpperLimitIs.ElementAt(0).lowerLimit)
                //{
                //    return false;
                //}
                //else //Fusionar dos regiones???
                //{
                //    return
                //}
                return false;
            }

            return false;
        }

        public double GetMaxWidthOfFreeRegions()
        {
            var maxWide = (double)0;
            //r=Max{Anchura de las regiones libres en el Gestor de Regiones de la XCelda-FUZZYMasteri}/2
            (double lowerLimit, double upperLimit) previousRegion = (0, 0);
            foreach (var region in ListOfRegions.OrderBy(reg => reg.lowerLimit).ToList())
            {
                var wide = region.lowerLimit - previousRegion.upperLimit;
                if (maxWide > wide)
                {
                    maxWide = wide;
                }
                previousRegion = region;
            }
            return maxWide;
        }

        private bool CheckIfValueIsInsideARegion((double lowerLimit, double upperLimit) region, double value)
            => value >= region.lowerLimit && value <= region.upperLimit;
    }
}
