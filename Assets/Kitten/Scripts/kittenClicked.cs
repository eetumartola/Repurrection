using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kittenClicked : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}

    void OnSelect()
    {
        Debug.Log("CLICKED!");
        transform.localPosition += new Vector3(0.0f, 0.1f, 0.0f);
    }
    
	// Update is called once per frame
	void Update ()
    {
		
	}
}
