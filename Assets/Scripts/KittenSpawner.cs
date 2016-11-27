using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenSpawner : MonoBehaviour
{
    public GameObject KittenPrefab;
    public float SpawnInterval = 2.0f;
    public int MaxSpawns = 25;
    public bool positioned = false;

    private float StartTime;
    private int instances = 0;

    void Start()
    {
        StartTime = Time.time;
    }

    public void Restart()
    {
        StartTime = Time.time;
        instances = 0;
    }

    void Update ()
    {
		if( Time.time - StartTime > SpawnInterval && instances < MaxSpawns && positioned)
        {
            GameObject newKitten = Instantiate(KittenPrefab, transform.position, Quaternion.Euler(0.0f, -90.0f, 0.0f));
            KittenController newKittenController = newKitten.GetComponent<KittenController>();
            newKittenController.instanceNumber = instances;
            instances++;
            GameManager.instance.AddSpawned();
            StartTime = Time.time;
        }
	}
}
