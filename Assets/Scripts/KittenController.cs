using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenController : MonoBehaviour
{
    private int direction = 1; // can be -1 or 1, depending on whether the kitten is moving along positive or negative X axis
    private Rigidbody rb;
    private float height = 1.0f;
    private float obstacleDist = 10.0f;
    private TextMesh tm;

    public int instanceNumber = 0;
    public float ForwardForce = 1.0f;
    public float ForceHeightLimit = 0.1f;
    public float ForwardVelocityLimit = 0.5f;
    public float ObstacleCollDistLimit = 0.02f;
    public GameObject DownRaycastPos;
    public GameObject FrontRaycastPos;
    public GameObject DebugText;


    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("No Rigidbody component in " + gameObject.name);

        if (DebugText != null)
        {
            tm = DebugText.GetComponent<TextMesh>();
            if (tm == null) Debug.LogWarning("No Textmesh Component in" + DebugText.name);
        }

        rb.centerOfMass = Vector3.zero;
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

        //Raycast Forwards to get kitten distance from obstacle
        RaycastHit forwardHitInfo;
        rayCastPos = FrontRaycastPos.transform.position;
        if (Physics.Raycast( rayCastPos, transform.forward, out forwardHitInfo ) )
        {
            obstacleDist = Vector3.Magnitude(FrontRaycastPos.transform.position - forwardHitInfo.point );
        }

        tm.text = "Kitty #" + instanceNumber + "\n height: " + height + "\n vel: " + rb.velocity + "\n dist: " + obstacleDist;

        if ( height < ForceHeightLimit )
        {
            rb.AddRelativeForce(ForwardForce * Vector3.forward);
        }
        if( Mathf.Abs(rb.velocity.x) > ForwardVelocityLimit )
        {
            rb.velocity = new Vector3(ForwardVelocityLimit * Mathf.Sign( rb.velocity.x), rb.velocity.y, 0.0f );
        }

        if ( obstacleDist < ObstacleCollDistLimit )
        {
            transform.localEulerAngles += new Vector3( 0.0f, 180f, 0.0f );
        }
    }

        void Update ()
    {
		
	}
}
