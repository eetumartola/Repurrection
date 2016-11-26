using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittenSpawner : MonoBehaviour
{
    public GameObject KittenPrefab;
    public float SpawnInterval = 2.0f;
    public int MaxSpawns = 25;

    private float StartTime;
    private int instances = 0;

	void Start ()
    {
		StartTime = Time.time;
	}
	
	void Update ()
    {
		if( Time.time - StartTime > SpawnInterval && instances < MaxSpawns )
        {
            GameObject newKitten = Instantiate(KittenPrefab, transform, true);
            KittenController newKittenController = newKitten.GetComponent<KittenController>();
            newKittenController.instanceNumber = instances;
            instances++;
            GameManager.instance.AddSpawned();
            StartTime = Time.time;
        }
	}
}
