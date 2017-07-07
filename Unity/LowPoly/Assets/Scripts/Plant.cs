using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSimulator
{
    public class Plant : MonoBehaviour
    {
        public Specie specie = null;                //!< Plant specie
        [SerializeField]
        Specimen specimen = null;
        
        // age related
        // -----------
        public float age = 0F;  //!< Current plant age
        public float biologicalAge = 0F;  //!< Current plant age on biologicla age (slowed down if dormant)
        public float seedingTime = 0F; //!< time since the specimen started seeding (when became mature)

        public float timeBeeingDominated = 0F; //!< time since the specimen is beeing dominated

        // size related
        // ------------
        public float height = 0F; //!< Current height
//        public float survivalRadius = 1F; //!< Radius at which the plant survives

        //// light/resources related
        //// -----------------------

        //public float canopyOpacity = 0.5F; //!< Percent of light reaching ground under the plant
        ////public float shadeTolerence = 0.0F; //!< Ability of the plant to grow in the shade of other
        //public float neededLightToGrow = 0.5F; //!< light factor need by the seed to start growing

        public int generation = 0;

        Collider mycollider = null;

        //public float CrownSize
        //{
        //    get
        //    {
        //        return (mycollider as CapsuleCollider).radius * transform.localScale.x;
        //    }
        //}

        public bool IsMature { get { return biologicalAge >= specimen.maturityAge; } }

        public Vector3 initScale = Vector3.one;

        // Use this for initialization
        void Awake()
        {
            //if (specimen == null)
                specimen = specie.GenerateSpecimen();

            initScale = transform.localScale;
            mycollider = GetComponent<Collider>();
        }

        public void Grow(float deltaTime)
        {
            if (domined > 0)
            {
                timeBeeingDominated += deltaTime;
                return;
            }

            biologicalAge += deltaTime;

            height += specimen.optimalGrowRate * deltaTime;
            if (height > specimen.adultHeight)
                height = specimen.adultHeight;

            ApplyDevelopement();
        }

        float colHeight = -1F;
        public void ApplyDevelopement()
        {
             if(colHeight < 0F)
                colHeight = (mycollider as CapsuleCollider).height;// * transform.localScale.x;
            float s = height / colHeight;
            transform.localScale = initScale * (s / initScale.y);
        }

        [SerializeField]
        int seeds = 0;
        void LateUpdate()
        {
            if (domined>0 && timeBeeingDominated < specie.survivalTimeDominated)
            {
                //Debug.Log("Die by domination");
                Destroy(gameObject);
                return;
            }

            float elapsedTime = Time.deltaTime * 30F; // 1 s is 10 years

            Grow(elapsedTime);
            
            if(IsMature)
            {
                if (seeds / seedingTime < specimen.successfullGerminationRate)
                    Seed();

                seedingTime += elapsedTime;
            }

            age += elapsedTime;
            if (age > specimen.lifeTime)
            {
                //Debug.Log("Died by age at " + age + " over " +specimen.lifeTime);
                GameObject.Destroy(gameObject);
            }
        }

        //void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("collision");
        //    //Destroy(other.gameObject);
        //}

        int domined = 0;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider == EcoSimulatorManager.Instance.water)
            {
                //Debug.Log("Drown");
                Destroy(gameObject);
                return;
            }
            if (collision.collider != EcoSimulatorManager.Instance.ground)
            {
                Plant concurrent = collision.collider.gameObject.GetComponent<Plant>();
                if (concurrent == null)
                    return;

                if (concurrent.height > height)
                    domined++;
                else
                    concurrent.domined++;
                //    Destroy(gameObject);//collision.collider.gameObject);
                //else
                //    Destroy(collision.collider.gameObject);
                if (domined <= 0)
                    timeBeeingDominated = 0F;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.collider != EcoSimulatorManager.Instance.ground)
            {
                Plant concurrent = collision.collider.gameObject.GetComponent<Plant>();
                if (concurrent == null)
                    return;

                if (concurrent.height > height)
                    domined--;
                else
                    concurrent.domined--;
                //    Destroy(gameObject);//collision.collider.gameObject);
                //else
                //    Destroy(collision.collider.gameObject);
                if (domined <= 0)
                    timeBeeingDominated = 0F;
            }
        }

        void Seed()
        {
            if (EcoSimulatorManager.Instance == null)
                return;

            seeds++;

            Vector3 pos = transform.position;

            //for (int i = 0; i < seedingCapacity; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-1F, 1F), 0, Random.Range(-1F, 1F));
                Vector3 seedPos = pos + (offset.normalized * Random.Range(0F, 1F)*specimen.successfullGerminationRadius);
                Ray ray = new Ray(seedPos + Vector3.up * 10000F, Vector3.down);
                //Debug.DrawRay(ray.origin, ray.direction * 100000F, Color.red, 1F);
                RaycastHit info;
                //if (!EcoSimulatorManager.Instance.ground.Raycast(ray, out info, 100000))
                //    return;// continue;
                //seedPos = ray.GetPoint(info.distance);
                {
                    RaycastHit[] all = Physics.RaycastAll(ray);
                    if (all.Length == 0)
                        return;

                    float waterDist = Mathf.Infinity;
                    float groundDist = Mathf.Infinity-1F;
                    foreach (RaycastHit rh in all)
                    {
                        if (rh.collider == EcoSimulatorManager.Instance.water)
                            waterDist = rh.distance;
                        if (rh.collider == EcoSimulatorManager.Instance.ground)
                        {
                            seedPos = ray.GetPoint(rh.distance);
                            groundDist = waterDist = rh.distance;
                        }
                    }

                    if (waterDist < groundDist)
                        return;
                }

                // if zero survival of seeds in shade
                ray = new Ray(seedPos, Vector3.up);
                if (Physics.Raycast(ray, out info, 50F))
                {
                    Plant above = info.collider.gameObject.GetComponent<Plant>();
                    if (above != null)
                    {
                        return;//continue;
                    }
                }

                GameObject child = Instantiate(gameObject);
                child.name = name;
                //child.transform.parent = transform;
                child.transform.position = seedPos;
                child.transform.localRotation = Quaternion.Euler(Random.Range(-5, 5), Random.Range(-180, 180), Random.Range(-5, 5));
                child.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

                Plant seed = child.GetComponent<Plant>();
                seed.specie = specie;
                seed.specimen = specimen.specie.GenerateSpecimen();
                seed.generation = generation + 1;
                seed.initScale = initScale;// * Random.Range(0.7F,1.5F);
                seed.height = 0F;
                seed.seedingTime = 0F;
                seed.age = 0F;
                seed.biologicalAge = 0F;
                seed.ApplyDevelopement();
            }
        }
    }
}
