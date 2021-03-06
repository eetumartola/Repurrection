﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenController : MonoBehaviour
{
    //private int direction = 1; // can be -1 or 1, depending on whether the kitten is moving along positive or negative X axis
    private Rigidbody rb;
    private float height = 1.0f;
    private float obstacleDist = 10.0f;
    private TextMesh tm;
    private float lastStableFloorHeight = 0.0f;
    private float stableFloorHeight = 0.0f;

    public int instanceNumber = 0;
    public float ForwardForce = 1.0f;
    public float ForceHeightLimit = 0.1f;
    public float ForwardVelocityLimit = 0.5f;
    public float ObstacleCollDistLimit = 0.02f;
    public float deadlyFallHeight = 0.5f;
    public GameObject DownRaycastPos;
    public GameObject FrontRaycastPos;
    public GameObject KittenDeathEffect;
    public GameObject KittenResurrectEffect;
    public GameObject DebugText;
    public bool showDebugText = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("No Rigidbody component in " + gameObject.name);

        if ( showDebugText && DebugText != null)
        {
            DebugText.SetActive(true);
            tm = DebugText.GetComponent<TextMesh>();
            if (tm == null) Debug.LogWarning("No Textmesh Component in" + DebugText.name);
        }

        lastStableFloorHeight = transform.position.y;

        rb.centerOfMass = new Vector3(0.0f, 0.05f, 0.0f);
	}

    private void FixedUpdate()
    {
        //rb.AddForce(ForwardForce * new Vector3(1.0f, 0.0f, 0.0f) * direction );

        //Raycast Downwards to get kitten height from ground
        RaycastHit downHitInfo;
        Vector3 rayCastPos = DownRaycastPos.transform.position;
        if (Physics.Raycast(rayCastPos, Vector3.down, out downHitInfo))
        {
            height = Vector3.Magnitude( transform.position - downHitInfo.point );
        }
        if (obstacleDist < ObstacleCollDistLimit)
        {
            transform.localEulerAngles += new Vector3(0.0f, 180f, 0.0f);
        }

        //Raycast Forwards to get kitten distance from obstacle
        RaycastHit forwardHitInfo;
        rayCastPos = FrontRaycastPos.transform.position; 
        //if (Physics.Raycast(rayCastPos, transform.forward, out forwardHitInfo))
        if (Physics.Raycast(rayCastPos, Mathf.Sign(rb.velocity.x) * Vector3.right, out forwardHitInfo))
        {
                obstacleDist = Vector3.Magnitude(FrontRaycastPos.transform.position - forwardHitInfo.point );
        }

        if (showDebugText)
        {
            tm.text = "Kitty #" + instanceNumber + "\n height: " + height + "\n dist: " + obstacleDist;
        }

        if ( height < ForceHeightLimit || Mathf.Abs(rb.velocity.y) < 0.1f )
        {
            rb.AddRelativeForce(ForwardForce * (Vector3.forward + 0.1f * Vector3.up));
            stableFloorHeight = transform.position.y;
            if( lastStableFloorHeight - stableFloorHeight > deadlyFallHeight)
            {
                Kill();
            }
            lastStableFloorHeight = stableFloorHeight;
        }
        else
        {
            rb.AddRelativeForce(ForwardForce * 0.1f * (Vector3.forward + 0.1f * Vector3.up));
        }

        if ( Mathf.Abs(rb.velocity.x) > ForwardVelocityLimit )
        {
            rb.velocity = new Vector3(ForwardVelocityLimit * Mathf.Sign( rb.velocity.x), rb.velocity.y, 0.0f );
        }

        if (transform.position.y < -20.0f) Kill();
    }

    public void Kill()
    {
        if (KittenDeathEffect != null)
        {
            GameObject fx = Instantiate(KittenDeathEffect, transform.position, KittenDeathEffect.transform.rotation);
            Destroy(fx, 2.0f);
        }
        GameManager.instance.AddDeath();
        Destroy(gameObject);
    }

    public void Resurrect()
    {
        if (KittenResurrectEffect != null)
        {
            GameObject fx = Instantiate(KittenResurrectEffect, transform.position, KittenResurrectEffect.transform.rotation);
            fx.transform.LookAt(Camera.main.transform);
            fx.transform.Rotate(new Vector3(0, 180, 0));
            Destroy(fx, 4.0f);
        }
        GameManager.instance.AddResurrection();
        Destroy(gameObject);
    }

    void Update ()
    {
		
	}
}
