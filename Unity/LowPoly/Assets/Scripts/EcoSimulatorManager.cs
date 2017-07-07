using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSimulator
{
    public class EcoSimulatorManager : MonoBehaviour
    {
        public static EcoSimulatorManager Instance = null;

        public Collider ground = null;
        public Collider water = null;
        // Use this for initialization
        void Start()
        {
            if (Instance == null)
                Instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}