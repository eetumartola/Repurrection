using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectRise : MonoBehaviour
{

    private float startTime = 0.0f;
    private Transform tr;
    private Vector3 startPos;

    public float riseAmount = 1.0f;
    public float riseTime = 2.0f;
    public bool killWhenFinished = true;

    void Start ()
    {
        startTime = Time.time;
        tr = transform;
        startPos = tr.localPosition;
	}
	
	void Update ()
    {
        float y = Mathf.Lerp(startPos.y, startPos.y + riseAmount, (Time.time - startTime) / riseTime );
        tr.localPosition = new Vector3(startPos.x, y, startPos.z);
        if( killWhenFinished && Time.time - startTime > riseTime)
        {
            Destroy(gameObject);
        }
	}
}
