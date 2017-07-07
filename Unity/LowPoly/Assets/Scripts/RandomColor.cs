using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bx
{
    [ExecuteInEditMode]
    public class RandomColor : MonoBehaviour
    {
        public Color RangeStart = Color.black;
        public Color RangeEnd = Color.white;

        public GameObject[] ToRandomize = null;

        // Use this for initialization
        void Awake ()
        {
            Randomize();
        }
	
	    // Update is called once per frame
	    void Update ()
        {
            //
                if (transform.hasChanged)
                {
                    transform.hasChanged = false;
                    Randomize();
                }
        }

        void Randomize()
        {
            foreach(GameObject o in ToRandomize)
            {
                if (o == null)
                    continue;
                Color c = RandomColorUtils.GetRandomColorInRange(RangeStart, RangeEnd, o.transform.position);
                foreach (Renderer r in o.GetComponentsInChildren<Renderer>())
                    r.material.color = c;
            }
        }
    }
}