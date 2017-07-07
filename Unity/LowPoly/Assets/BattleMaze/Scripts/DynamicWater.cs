using UnityEngine;
using System.Collections;

public class DynamicWater : MonoBehaviour {

    Material waveMaterial;
    GlobalReferences globals;
    //public GameObject splashPrefab;

	// Use this for initialization
	void Start ()
    {
        globals = GameObject.Find("GlobalReferences").GetComponent<GlobalReferences>();
        waveMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < globals.wavesIsMoving.Length; i++)
        {
            if (globals.wavesIsMoving[i])
            {
                globals.collisionVectors[i].w = globals.waveTimers[i] * 0.01f;
                waveMaterial.SetVector("_CollisionVectors" + i.ToString(), globals.collisionVectors[i]);
                if (globals.collisionVectors[i].w >= 1.0f)
                {
                    globals.wavesIsMoving[i] = false;
                    globals.collisionVectors[i].w = 0.0f;
                    globals.waveTimers[i] = 0.0f;
                }
                globals.waveTimers[i] += Time.deltaTime;
            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Box")
        {
            //GameObject splash = Instantiate(splashPrefab, coll.transform.position, Quaternion.identity) as GameObject;
            //splash.transform.rotation = Quaternion.EulerAngles(-90, 0, 0);
            //Destroy(splash, 2.0f);
            Debug.Log("Splash");
            Rigidbody rigB = collision.collider.GetComponent<Rigidbody>();
            globals.wavesIsMoving[globals.waveCounter] = true;
            globals.waveTimers[globals.waveCounter] = 0.0f;
            globals.collisionVectors[globals.waveCounter].x = collision.collider.transform.position.x;
            globals.collisionVectors[globals.waveCounter].y = collision.collider.transform.position.z;
            globals.collisionVectors[globals.waveCounter].z = rigB.velocity.y * rigB.mass * 0.01f;
            globals.collisionVectors[globals.waveCounter].w = 0.0f;
            globals.waveCounter++;
            if(globals.waveCounter >= 20)
            {
                globals.waveCounter = 0;
            }

        }
    }
}
