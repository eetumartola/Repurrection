using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenGoal : MonoBehaviour
{

	void Start ()
    {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Goal entered by " + collider.gameObject.name);
        KittenController kc = collider.gameObject.GetComponentInParent<KittenController>();
        kc.Resurrect();
        //mc.AddMiss();
    }
}
