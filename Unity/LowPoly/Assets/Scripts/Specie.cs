using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSimulator
{
    [System.Serializable]
    public class FloatRange
    {
        public Vector2 range = new Vector2(0F, 0F);
        public bool Parse(string s)
        {
            return true;
        }
    }
    //[System.Serializable]
    public class Specie : MonoBehaviour
    {
        public string binomialName = "";
        public List<string> frenchNames = null;
        public List<string> englishNames = null;

        public FloatRange lifeTime = null;                      //!< Life time range in years
        public FloatRange adultHeight = null;                        //!< Usual adult height range in meters

        public float optimalGrowRate = 0F;                      //!< Usual grow rate under optimal conditions (in meters per year)

        //public float width;                                   //!< Usual adult width in meters

        public FloatRange altitude = null;                      //!< Usual altitude range in meters

        public FloatRange maturityAgeRange = null;                 //!< Usual maturity age at which a specimen starts seeding (in years)
        public FloatRange successfullGerminationRate = null;        //!< Number of successfull sprouts per years

        public FloatRange successfullGerminationRadius = null;  //!< Radius at which there is a successful sprout

        public float survivalTimeDominated = 0F;                //!< Time a specimen can survive beeing dominated by other plants (in years)

        public Specimen GenerateSpecimen()
        {
            return new Specimen()
            {
                specie = this,
                lifeTime = Random.Range(lifeTime.range.x, lifeTime.range.y),
                adultHeight = Random.Range(adultHeight.range.x, adultHeight.range.y),
                optimalGrowRate = optimalGrowRate,
                maturityAge = Random.Range(maturityAgeRange.range.x, maturityAgeRange.range.y),
                successfullGerminationRate = Random.Range(successfullGerminationRate.range.x, successfullGerminationRate.range.y),
                successfullGerminationRadius = Random.Range(successfullGerminationRadius.range.x, successfullGerminationRadius.range.y),
            };
        }
    }

    [System.Serializable]
    public class Specimen
    {
        public Specie specie = null;                //!< Specimen specie

        public float lifeTime;                      //!< Life time range in years
        public float adultHeight;                   //!< Usual adult height range in meters

        public float optimalGrowRate;               //!< Usual grow rate under optimal conditions (in meters per year)
        //public float width;                       //!< Usual adult width in meters

        public float maturityAge;                   //!< Usual maturity age at which a specimen starts seeding (in years)

        public float successfullGerminationRate;    //!< Number of successfull sprouts per years
        public float successfullGerminationRadius;  //!< Radius at which there is a successful sprout
    }
}