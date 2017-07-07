using UnityEngine;
using System.Collections;

public class GlobalReferences : MonoBehaviour {

    public int waveCounter;
    public bool[] wavesIsMoving;
    public float[] waveTimers;
    public Vector4[] collisionVectors;

    void Start()
    {
        waveCounter = 0;
        wavesIsMoving = new bool[20];
        waveTimers = new float[20];
        collisionVectors = new Vector4[20];
        for (int i = 0; i < collisionVectors.Length; i++)
        {
            collisionVectors[i] = new Vector4(0, 0, 0, 0);
        }
    }
}
