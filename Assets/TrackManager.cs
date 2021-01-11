//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs;
    public float zSpawn = 0;
    public float trackLength = 50;
    public Transform playerTransform;
    int nTrack;
    List<GameObject> activeTracks = new List<GameObject>();
    void Start()
    {
        nTrack = trackPrefabs.Length;
        SpawnTrack();
        SpawnTrack();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;


    }


    void Update()
    {
        if (playerTransform.position.z - 30 > zSpawn - (nTrack * trackLength))
        {
            SpawnTrack1();
            Debug.Log(playerTransform.position.z);
            Debug.Log(zSpawn - (nTrack * trackLength));
            DeleteTrack();

        }
    }
    public void SpawnTrack()
    {
        Vector3 position = new Vector3(0, -16.662f, zSpawn);
        GameObject go = Instantiate(trackPrefabs[0], position, transform.rotation);
        activeTracks.Add(go);
        zSpawn += trackLength;
    }
    public void SpawnTrack1()
    {
        Quaternion target = Quaternion.Euler(60.06f, 0, 0);
        float smooth = 5.0f;


        Vector3 position = new Vector3(0, -16.662f, zSpawn);
        GameObject go = Instantiate(trackPrefabs[0], position, transform.rotation);
        activeTracks.Add(go);
        zSpawn += trackLength;
    }

    void DeleteTrack()
    {
        Destroy(activeTracks[0]);
        activeTracks.RemoveAt(0);
    }

}
