using UnityEngine;
using System.Collections;

public class ResetCube : MonoBehaviour {

    Rigidbody rigB;
    Transform trans;
    
	// Use this for initialization
	void Start () {
        rigB = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(rigB.position.y < -5)
        {
            float scaler = Random.Range(0.5f, 3.0f);
            trans.localScale = new Vector3(1.0f * scaler, 1.0f * scaler, 1.0f * scaler);
            rigB.mass = scaler;
            rigB.velocity = Vector3.zero;
            rigB.MovePosition(new Vector3(Random.Range(5, 20), Random.Range(20, 30), Random.Range(0, 30)));
        }
	}
}
