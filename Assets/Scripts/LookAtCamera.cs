using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    public Vector3 RotationOffset = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(RotationOffset);
	}
}
