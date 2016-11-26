using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenGoal : MonoBehaviour
{
	void Start ()
    {
		
	}

    public void Position()
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        Vector3 dropPosition = headPosition + 2.0f * gazeDirection;
        Vector3 dropDirection = Vector3.down;

        RaycastHit hitInfo;
        if (Physics.Raycast(dropPosition, dropDirection, out hitInfo))
        {
            transform.position = hitInfo.point;            
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            transform.position = dropPosition;
        }


    }


    private void OnTriggerEnter(Collider collider)
    {
       // Debug.Log("Goal entered by " + collider.gameObject.name);
        KittenController kc = collider.gameObject.GetComponentInParent<KittenController>();
        kc.Resurrect();
    }
}
