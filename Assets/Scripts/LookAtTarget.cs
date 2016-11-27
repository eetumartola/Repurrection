using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(0.5f * Mathf.Sin(Time.time), 0.0f, 0.0f);
        if (target != null) transform.LookAt(target.transform);
        //else Debug.Log("No target!");
    }

}
